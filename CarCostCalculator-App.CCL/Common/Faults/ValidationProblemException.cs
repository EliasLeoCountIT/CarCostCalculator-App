namespace CarCostCalculator_App.CCL.Common.Faults
{
    /// <summary>
    /// Represents an exception that is thrown when a validation problem occurs.
    /// </summary>
    /// <param name="title">A text that summarizes the error.</param>
    /// <param name="errors"> Errors that occurred during validation.</param>
    /// <param name="type">Type that caused validation errors.</param>
    public class ValidationProblemException(string title, IDictionary<string, string[]> errors, Type type)
        : ProblemException(title, $"{errors.Values.Count} errors occurred during the validation of {type.Name}.")
    {
        #region Public Properties

        /// <summary>
        /// Errors that occurred during validation.
        /// </summary>
        public IDictionary<string, string[]> Errors { get; set; } = errors;

        #endregion
    }
}
