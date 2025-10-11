using CarCostCalculator_App.CCL.Common.Faults;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Car_Cost_Calculator_App.Dialogs
{
    public partial class CategoryDialog
    {
        [CascadingParameter]
        public IMudDialogInstance? MudDialog { get; set; }
        [Inject]
        public required ISender Sender { get; set; }
        [Inject]
        public required ISnackbar Snackbar { get; set; }
        [Parameter]
        public required CategoryCore Category { get; set; }

        private void Cancel() => MudDialog?.Cancel();

        private async Task OnValidSubmit()
        {
            try
            {
                CategoryCore? response;

                if (Category.Id == 0)
                {
                    response = await Sender.Send(new CreateCategory(Category));
                }
                else
                {
                    response = await Sender.Send(new UpdateCategory(Category));
                }


                MudDialog?.Close(DialogResult.Ok(response));

                Snackbar.Add("Kategorie erfolgreich aktualisiert.", Severity.Success);
            }
            catch (RemoteProblemException ex)
            {
                Snackbar.Add(ex.Details.Detail!, Severity.Error);
            }
        }
    }
}