using CarCostCalculator_App.CCL.Common.Authentication;
using CarCostCalculator_App.CCL.Common.Authorization;

namespace CarCostCalculator_App.CCL.CQRS.Authorization
{
    /// <summary>
    /// Defines methods to handle authorization.
    /// </summary>
    /// <param name="userProvider">Provides access to the current user</param>
    public class AuthorizationService(IUserProvider userProvider)
    {
        #region Private Members

        private readonly IUserProvider _userProvider = userProvider;

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the user has any of the given role groups
        /// </summary>
        /// <param name="groupedRoles">Collection of role groups that are required to be authorized.</param>
        /// <returns>Returns <c>true</c> if the user has the all requested roles in any of the given groups.</returns>
        public bool IsAuthorized(IEnumerable<string[]> groupedRoles) => groupedRoles.Aggregate(false, (current, requiredRoles) => current || IsAuthorized(requiredRoles));

        /// <summary>
        /// Checks if the user has all given roles
        /// </summary>
        /// <param name="requiredRoles">All roles that are required to be authorized</param>
        /// <returns>Returns <c>true</c> if the user has the all requested roles.</returns>
        public bool IsAuthorized(params string[] requiredRoles) => Array.TrueForAll(requiredRoles, UserHasRole);

        #endregion

        #region Private Methods

        private bool UserHasRole(string role) => _userProvider.User.CheckRequiredRole(role);

        #endregion
    }
}
