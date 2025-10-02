using System.Security.Principal;

namespace CarCostCalculator_App.CCL.Common.Authentication
{
    /// <summary>
    /// Provides the current user for authentication purposes.
    /// </summary>
    public interface IUserProvider
    {
        #region Public Properties

        /// <summary>
        /// Gets the authenticated user principal.
        /// </summary>
        IPrincipal User { get; }

        #endregion
    }
}
