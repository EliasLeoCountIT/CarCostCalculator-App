namespace CarCostCalculator_App.CCL.Contracts
{
    /// <summary>
    /// Marker interface to represent a idempotent command with a void response.
    /// </summary>
    public interface IIdempotentCommand : ICommand
    { }

    /// <summary>
    /// Marker interface to represent a idempotent command with a response.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IIdempotentCommand<out TResponse> : ICommand<TResponse>
    { }
}
