using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.Models;
using MediatR;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.OData
{
    /// <summary>
    /// Wrap the <paramref name="Query"/> to provide <see cref="PagedDataResult{TItem}"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    /// <param name="Query">The underlying OData query.</param>
    public record ODataQueryWrapper<TItem>(IODataQuery<TItem> Query) : IRequest<PagedDataResult<TItem>>
    { }
}
