using MediatR;

namespace CarCostCalculator_App.CCL.Contracts
{
    /// <summary>
    /// Marker interface to represent a command with a void response.
    /// </summary>
    public interface ICommand : IRequest, IWebRequest
    { }

    /// <summary>
    /// Marker interface to represent a command with a response.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse>, IWebRequest
    { }
}
