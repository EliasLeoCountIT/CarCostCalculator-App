using MediatR;

namespace CarCostCalculator_App.CCL.Contracts
{
    /// <summary>
    /// Marker interface to represent a query.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IQuery<out TResponse> : IRequest<TResponse>, IWebRequest;
}
