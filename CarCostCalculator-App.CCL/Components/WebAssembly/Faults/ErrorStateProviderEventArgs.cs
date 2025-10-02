namespace CarCostCalculator_App.CCL.Components.WebAssembly.Faults
{
    /// <summary>
    /// Event arguments for exception.
    /// <paramref name="exception">Exception to handle.</paramref>
    /// </summary>
    public class ErrorStateProviderEventArgs(Exception exception) : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// The exception.
        /// </summary>
        public Exception Exception { get; } = exception;

        #endregion
    }
}
