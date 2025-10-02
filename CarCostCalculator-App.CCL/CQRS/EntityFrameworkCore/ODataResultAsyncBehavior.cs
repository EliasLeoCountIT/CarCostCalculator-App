using CarCostCalculator_App.CCL.CQRS.AspNetCore.OData;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CarCostCalculator_App.CCL.CQRS.EntityFrameworkCore
{
    /// <summary>
    /// Behavior to perform <see cref="EntityFrameworkQueryableExtensions.ToListAsync"/> if supported.
    /// </summary>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    public class ODataResultAsyncBehavior<TItem> : IPipelineBehavior<ODataResultRequest<TItem>, PagedDataResult<TItem>>
    {
        #region Public Methods

        /// <summary>
        /// Performs <see cref="EntityFrameworkQueryableExtensions.ToListAsync"/> if supported, delegates to <paramref name="next"/> if not.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TItem"/>.</returns>
        public async Task<PagedDataResult<TItem>> Handle(ODataResultRequest<TItem> request, RequestHandlerDelegate<PagedDataResult<TItem>> next, CancellationToken cancellationToken)
        {
            var supportsAsync = request.Queryable.Provider is IAsyncQueryProvider;

            if (supportsAsync)
            {
                //use async query
                var items = await request.Queryable.ToListAsync(cancellationToken);

                var result = new PagedDataResult<TItem>(items, request.TotalCount ?? 0, request.Query.Top, request.Query.Skip);

                return result;
            }
            else
            {
                //use default handler
                return await next(cancellationToken);
            }
        }

        #endregion
    }
}
