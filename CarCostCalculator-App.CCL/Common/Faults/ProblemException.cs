namespace CarCostCalculator_App.CCL.Common.Faults
{
    /// <summary>
    /// Represents an exception that is thrown when a runtime problem occurs.
    /// </summary>
    /// <param name="title">A text that summarizes the error.</param>
    /// <param name="message">A message that describes the problem.</param>
    public class ProblemException(string title, string message) : Exception(message)
    {
        #region Public Properties

        /// <summary>
        /// A text that summarizes the error.
        /// </summary>
        public string Error { get; } = title;

        #endregion
    }
}
