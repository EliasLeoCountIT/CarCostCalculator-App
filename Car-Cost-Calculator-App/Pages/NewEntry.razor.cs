using CarCostCalculator_App.CCL.CQRS.HTTP.Client.MediatR;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Car_Cost_Calculator_App.Pages
{
    public partial class NewEntry
    {
        private string? _value;
        private bool _success => !string.IsNullOrWhiteSpace(_value) && _selectedOption != null;
        private MudForm? _form;
        private DateTime? _dateOfPayment = DateTime.Today;
        private string _selectedOption = "Preis";
        private double _priceOrKilometer;

        private List<Category>? _statesOfCategories;

        [Inject]
        public required ISender Sender { get; set; }
        private async Task AddNewEntryButtonClick()
        {
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var result = await Sender.SendOData(new CategoriesViaOData { Top = int.MaxValue });

            _statesOfCategories = [.. result.Items];
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
            _value = value;
            StateHasChanged();
        }

    }
}