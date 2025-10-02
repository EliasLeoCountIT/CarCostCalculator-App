using System.Security.Principal;

namespace CarCostCalculator_App.CCL.Common.Authentication
{
    /// <summary>
    /// Provides authentication access.
    /// </summary>
    public interface IAuthenticationAccessor
    {
        #region Public Properties

        /// <summary>
        /// Gets the associated user principal.
        /// </summary>
        IPrincipal Principal { get; }

        /// <summary>
        /// Gets the current state of the <see cref="IAuthenticationAccessor"/>.
        /// </summary>
        AuthenticationAccessState State { get; }

        #endregion
    }
}
