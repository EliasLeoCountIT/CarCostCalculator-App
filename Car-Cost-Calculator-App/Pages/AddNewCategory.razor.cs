using CarCostCalculator_App.CCL.CQRS.HTTP.Client.MediatR;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace Car_Cost_Calculator_App.Pages
{
    public partial class AddNewCategory
    {
        private List<CategoryCore>? _allCategories;
        private string? _searchString;

        [Inject]
        public required ISender Sender { get; set; }

        [Inject]
        public required ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            var result = await Sender.SendOData(new CategoriesViaOData { Top = int.MaxValue });
            _allCategories = [.. result.Items];
        }

        private Func<CategoryCore, bool> QuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void UpdateCategory()
        {

        }

        private async Task DeleteCategory(int id)
        {
            try
            {
                await Sender.Send(new DeleteCategory(id));
                await LoadCategories();
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}