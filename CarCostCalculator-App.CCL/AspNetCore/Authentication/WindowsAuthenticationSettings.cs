namespace CarCostCalculator_App.CCL.AspNetCore.Authentication
{
    /// <summary>
    /// Provides settings for windows authentication.
    /// </summary>
    public class WindowsAuthenticationSettings
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the available groups of this application.
        /// </summary>
        public required IEnumerable<string> AvailableGroups { get; set; }

        #endregion
    }
}
