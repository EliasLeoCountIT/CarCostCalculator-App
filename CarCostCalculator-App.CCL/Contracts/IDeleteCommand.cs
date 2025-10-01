namespace CarCostCalculator_App.CCL.Contracts
{
    /// <summary>
    /// Marker interface to represent a delete command with a void response.
    /// </summary>
    public interface IDeleteCommand : ICommand
    { }

    /// <summary>
    /// Marker interface to represent a delete command with a response.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface IDeleteCommand<out TResponse> : ICommand<TResponse>
    { }
}
