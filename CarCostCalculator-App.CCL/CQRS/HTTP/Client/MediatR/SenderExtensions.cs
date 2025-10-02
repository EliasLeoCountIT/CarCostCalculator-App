using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.OData;
using CarCostCalculator_App.CCL.Models;
using MediatR;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.MediatR
{
    /// <summary>
    /// Extension methods for <see cref="ISender"/>.
    /// </summary>
    public static class SenderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Send an OData query request.
        /// </summary>
        /// <typeparam name="TItem">The type of the requested items.</typeparam>
        /// <param name="sender">Send a request through the mediator pipeline to be handled by a single handler.</param>
        /// <param name="request">The OData query request.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
        public static Task<PagedDataResult<TItem>> SendOData<TItem>(this ISender sender, IODataQuery<TItem> request, CancellationToken cancellationToken = default)
            => sender.Send(new ODataQueryWrapper<TItem>(request), cancellationToken);

        #endregion
    }
}
