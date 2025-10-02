using CarCostCalculator_App.CCL.Common.Faults;
using CarCostCalculator_App.CCL.Components.WebAssembly.Localization;
using MediatR;
using MudBlazor;

namespace CarCostCalculator_App.CCL.Components.WebAssembly.CQRS
{
    /// <summary>
    /// Represents a behavior that catches <see cref="RemoteProblemException"/> and informs the user.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request to authorize.</typeparam>
    /// <typeparam name="TResponse">Response type of the request.</typeparam>
    internal class RemoteProblemExceptionBehavior<TRequest, TResponse>(ISnackbar snackbar, IAppLocalizer stringLocalizer) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        #region Private Members

        private readonly ISnackbar _snackbar = snackbar;
        private readonly IAppLocalizer _stringLocalizer = stringLocalizer;

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="RemoteProblemException"/> pipeline handler.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TResponse"/>.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next(cancellationToken);
            }
            catch (RemoteProblemException ex)
            {
                _snackbar.Add(_stringLocalizer[ex.Details.Title ?? ex.Message], Severity.Error, configure => configure.RequireInteraction = true);

                throw;
            }
        }

        #endregion
    }
}
