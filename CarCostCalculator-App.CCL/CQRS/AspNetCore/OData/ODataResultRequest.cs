using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.Models;
using MediatR;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.OData
{
    /// <summary>
    /// Retrieves the result of the specified query.
    /// </summary>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    /// <param name="Query">An open data protocol query that requests <typeparamref name="TItem"/>.</param>
    /// <param name="Queryable">The <see cref="IQueryable{TItem}"/> of the requested items.</param>
    /// <param name="TotalCount">The total count of available items.</param>
    public record class ODataResultRequest<TItem>(IODataQuery<TItem> Query, IQueryable<TItem> Queryable, long? TotalCount) : IRequest<PagedDataResult<TItem>>
    { }
}
