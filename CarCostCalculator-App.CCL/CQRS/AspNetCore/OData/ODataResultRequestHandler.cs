using CarCostCalculator_App.CCL.Models;
using MediatR;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.OData
{
    /// <summary>
    /// Defines the handler for <see cref="ODataResultRequest{TItem}"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    public class ODataResultRequestHandler<TItem> : IRequestHandler<ODataResultRequest<TItem>, PagedDataResult<TItem>>
    {
        #region Public Methods

        /// <summary>
        /// Handles a <see cref="ODataResultRequest{TItem}"/>.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <see cref="PagedDataResult{TItem}"/>.</returns>
        public Task<PagedDataResult<TItem>> Handle(ODataResultRequest<TItem> request, CancellationToken cancellationToken)
        {
            var items = request.Queryable.ToList();

            var result = new PagedDataResult<TItem>(items, request.TotalCount ?? 0, request.Query.Top, request.Query.Skip);

            return Task.FromResult(result);
        }

        #endregion
    }
}
