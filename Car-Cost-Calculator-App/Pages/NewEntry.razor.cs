using CarCostCalculator_App.CCL.CQRS.HTTP.Client.MediatR;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Contract.KilometerEntry;
using CarCostCalculator_App.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Car_Cost_Calculator_App.Pages
{
    public partial class NewEntry
    {
        private MudForm? _form;
        private List<CategoryCore>? _statesOfCategories;

        private DateTime? _dateOfPayment;
        private string _selectedOption = null!;
        private string? _selectedCategory;
        private double _valueToAdd;

        private bool _success => !string.IsNullOrWhiteSpace(_selectedCategory) && _selectedOption != null;
        private bool _isLoading = true;



        [Inject]
        public required ISender Sender { get; set; }

        [Inject]
        public required ISnackbar Snackbar { get; set; }

        private async Task AddNewEntryButtonClick()
        {
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            try
            {
                var entry = new KilometerEntryCore
                {
                    Kilometers = _valueToAdd,
                    PaymentDate = _dateOfPayment!.Value,
                };

                await Sender.Send(new AddKilometerEntry(entry));
                Snackbar.Add($"Eintrag wurde erfolgreich hinzugefügt.", Severity.Success);
            }
            catch (Exception e)
            {
                Snackbar.Add($"Hinzufügen fehlgeschlagen: {e.Message}", Severity.Error);
            }

        }

        private void LoadFormValues()
        {
            _valueToAdd = 0.0;
            _dateOfPayment = DateTime.Today;
            _selectedOption = "Preis";
        }
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            await base.OnInitializedAsync();

            var result = await Sender.SendOData(new CategoriesViaOData { Top = int.MaxValue });

            _statesOfCategories = [.. result.Items];
            LoadFormValues();

            _isLoading = false;
            StateHasChanged();
        }
        private string? ValidateCategory(string? c)
        {
            return string.IsNullOrEmpty(c) ? "Bitte wähle eine Kategorie aus" : null;
        }

        private string? ValidatePrice(double p)
        {
            return p > 20_000.00 ? "Der bezahlte Preis darf nicht größer als 20.000 sein" : null;
        }

        private void SelectedCategoryChanged(string value)
        {
            _selectedCategory = value;
            StateHasChanged();
        }

    }
}