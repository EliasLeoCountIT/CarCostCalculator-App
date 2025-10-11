using Car_Cost_Calculator_App.Dialogs;
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
        private MudDataGrid<CategoryCore>? _grid;
        private List<CategoryCore>? _allCategories;
        private bool _isLoading = true;
        private string? _searchString;

        [Inject]
        public required ISender Sender { get; set; }
        [Inject]
        public required ISnackbar Snackbar { get; set; }
        [Inject]
        public required IDialogService DialogService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            _isLoading = true;
            var result = await Sender.SendOData(new CategoriesViaOData { Top = int.MaxValue });
            _allCategories = [.. result.Items];
            _isLoading = false;
        }

        private Func<CategoryCore, bool> QuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private async Task AddCategory()
        {
            try
            {
                var dialog = await DialogService.ShowAsync<CategoryDialog>("Kategorie hinzufügen",
                    new DialogParameters
                    {
                        ["Category"] = new CategoryCore { Name = string.Empty }
                    },
                    new DialogOptions { CloseOnEscapeKey = true, CloseButton = true, BackdropClick = true, MaxWidth = MaxWidth.Medium });
                var result = await dialog.Result;
                if (_grid is not null)
                {
                    await LoadCategories();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Hinzufügen fehlgeschlagen: {ex.Message}", Severity.Error);
            }
        }

        private async Task EditCategory(CategoryCore category)
        {
            try
            {
                var dialog = await DialogService.ShowAsync<CategoryDialog>("Kategorie bearbeiten",
                    new DialogParameters
                    {
                        ["Category"] = category
                    },
                    new DialogOptions { CloseOnEscapeKey = true, CloseButton = true, BackdropClick = true, MaxWidth = MaxWidth.Medium });

                var result = await dialog.Result;

                if (_grid is not null)
                {
                    await LoadCategories();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Löschen fehlgeschlagen: {ex.Message}", Severity.Error);
            }
        }

        private async Task DeleteCategory(int id)
        {
            try
            {
                await Sender.Send(new DeleteCategory(id));
                await LoadCategories();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Löschen fehlgeschlagen: {ex.Message}", Severity.Error);
            }

        }

    }
}