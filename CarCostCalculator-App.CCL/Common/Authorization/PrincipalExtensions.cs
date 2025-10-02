using System.Security.Claims;
using System.Security.Principal;

namespace CarCostCalculator_App.CCL.Common.Authorization
{
    /// <summary>
    /// <see cref="IPrincipal"/> extension methods.
    /// </summary>
    public static class PrincipalExtensions
    {
        #region Public Methods

        /// <summary>
        /// Checks if the <see cref="IPrincipal"/> has the required role assigned.
        /// </summary>
        /// <param name="principal"><see cref="IPrincipal"/></param>
        /// <param name="requiredRole">Security identifier of the required role.</param>
        /// <returns>Returns <c>true</c> if the user is assigned to the required role.</returns>
        public static bool CheckRequiredRole(this IPrincipal principal, string requiredRole)
        {
            if (principal is ClaimsPrincipal claimsPrincipal)
            {
                return string.IsNullOrWhiteSpace(requiredRole)
                       || claimsPrincipal.IsInRole(requiredRole);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
