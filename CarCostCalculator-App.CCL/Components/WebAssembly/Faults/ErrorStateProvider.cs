namespace CarCostCalculator_App.CCL.Components.WebAssembly.Faults
{
    /// <summary>
    /// Provides functionality for handling and notifying about exceptions.
    /// </summary>
    public class ErrorStateProvider
    {
        #region Public Events

        /// <summary>
        /// Event triggered when an error occurs.
        /// </summary>
        public event Action<ErrorStateProviderEventArgs>? ErrorOccurred;

        #endregion

        #region Public Methods

        /// <summary>
        /// Raises the <see cref="ErrorOccurred"/> event.
        /// </summary>
        /// <param name="args">The event arguments containing error details.</param>
        public void OnErrorOccurred(ErrorStateProviderEventArgs args)
            => ErrorOccurred?.Invoke(args);

        #endregion
    }
}
