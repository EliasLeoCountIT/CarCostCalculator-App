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
        private const string _placeHolderValue = "-";

        private MudForm? _form;
        private MudSelect<string>? _categorySelect;
        private List<CategoryCore>? _allCategories;

        private DateTime? _dateOfPayment;
        private string _selectedRadioBtn = null!;
        private string? _selectedCategory;
        private double _valueToAdd;


        private bool Success => !string.IsNullOrWhiteSpace(_selectedCategory) && _selectedRadioBtn != null;
        private bool IsLoading = true;



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
                if (_selectedRadioBtn == "Preis")
                {
                    var category = _allCategories!.FirstOrDefault(c => c.Name == _selectedCategory);

                    var entry = new CostEntryCore
                    {
                        CategoryId = category!.Id,
                        Price = _valueToAdd,
                        PaymentDate = _dateOfPayment!.Value,
                    };
                }
                else
                {
                    var entry = new KilometerEntryCore
                    {
                        Kilometers = _valueToAdd,
                        PaymentDate = _dateOfPayment!.Value,
                    };

                    await Sender.Send(new AddKilometerEntry(entry));
                }

                Snackbar.Add($"Eintrag wurde erfolgreich hinzugefügt.", Severity.Success);
            }
            catch (Exception e)
            {
                Snackbar.Add($"Hinzufügen fehlgeschlagen: {e.Message}", Severity.Error);
            }

        }

        private void SelectedRadioBtnChanged(string value)
        {
            if (value == "Kilometer")
            {
                _selectedCategory = _placeHolderValue;
                InvokeAsync(async () =>
                {
                    StateHasChanged();
                    await Task.Delay(1);
                    _categorySelect?.Validate();
                });
            }
            else
            {
                _selectedCategory = null;
                StateHasChanged();
            }

            _selectedRadioBtn = value;
        }

        private void LoadFormValues()
        {
            _valueToAdd = 0.0;
            _dateOfPayment = DateTime.Today;
            _selectedRadioBtn = "Preis";
            _selectedCategory = null;
        }
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await base.OnInitializedAsync();

            var result = await Sender.SendOData(new CategoriesViaOData { Top = int.MaxValue });

            _allCategories = [.. result.Items];
            LoadFormValues();

            IsLoading = false;
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