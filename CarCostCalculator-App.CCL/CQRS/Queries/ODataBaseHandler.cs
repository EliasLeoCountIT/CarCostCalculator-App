using CarCostCalculator_App.CCL.CQRS.Abstractions.Queries;
using MediatR;

namespace CarCostCalculator_App.CCL.CQRS.Queries
{
    /// <summary>
    /// Defines abstract base handler for a open data protocol query.
    /// </summary>
    /// <typeparam name="TQuery">The type of <see cref="IODataQuery{TItem}"/> being handled.</typeparam>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    public abstract class ODataBaseHandler<TQuery, TItem> : IRequestHandler<TQuery, IQueryable<TItem>>
        where TQuery : ODataBaseQuery<TItem>
    {
        #region Public Methods

        /// <summary>
        /// Handles a open data protocol query.
        /// </summary>
        /// <param name="request">The open data protocol query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the query.</returns>
        public Task<IQueryable<TItem>> Handle(TQuery request, CancellationToken cancellationToken) => Task.FromResult(RetrieveQueryable(request));

        #endregion

        #region Protected Methods

        /// <summary>
        /// Retrieve <see cref="IQueryable{TItem}"/> for the open data protocol query.
        /// </summary>
        /// <param name="request">The open data protocol query.</param>
        /// <returns><see cref="IQueryable{TItem}"/></returns>
        protected abstract IQueryable<TItem> RetrieveQueryable(TQuery request);

        #endregion
    }
}
