using CarCostCalculator_App.CCL.Common.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;
using System.Security.Principal;

namespace CarCostCalculator_App.CCL.AspNetCore.Authentication
{
    /// <inheritdoc cref="IAuthenticationAccessor"/>
    public class HttpContextAuthenticationAccessor(IHttpContextAccessor httpContextAccessor) : IAuthenticationAccessor
    {
        #region Private Members

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        #endregion

        #region Public Properties

        /// <inheritdoc/>
        public IPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? throw new AuthenticationException("There is no context available to provide the current user.");

        /// <inheritdoc/>
        public AuthenticationAccessState State => _httpContextAccessor.HttpContext is not null ? AuthenticationAccessState.Active : AuthenticationAccessState.Inactive;

        #endregion
    }
}
