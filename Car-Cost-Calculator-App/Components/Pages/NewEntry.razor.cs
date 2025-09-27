using MudBlazor;

namespace Car_Cost_Calculator_App.Components.Pages
{
    public partial class NewEntry
    {
        private string? _value;
        private bool _success => !string.IsNullOrWhiteSpace(_value) && _selectedOption != null;
        private MudForm? _form;
        private DateTime? _dateOfPayment = DateTime.Today;
        private string _selectedOption = "Preis";
        private double _priceOrKilometer;

        private readonly string[] _statesOfCategories =
        {
            "Kfz-Versicherung", "Zulassung", "Pickerl", "Service", "ÖAMTC",
            "Tanken", "Vignette", "Sonstiges"
        };
        private async Task AddNewEntryButtonClick()
        {
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

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