namespace CarCostCalculator_App.CCL.Common.Faults
{
    /// <summary>
    /// Represents an exception that is thrown when a configuration is missing.
    /// </summary>
    /// <param name="message">A message that describes the exception.</param>
    public class MissingConfigurationException(string message) : SystemException(message);
}
