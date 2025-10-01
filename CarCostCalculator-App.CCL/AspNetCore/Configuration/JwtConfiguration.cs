using CarCostCalculator_App.CCL.Common.Faults;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarCostCalculator_App.CCL.AspNetCore.Configuration
{
    /// <summary>
    /// Configuration for JWT.
    /// </summary>
    public class JwtConfiguration
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the audience of the JWT.
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// Get or sets if the token is expiring.
        /// </summary>
        public bool IsExpiring { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the JWT.
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// Gets or sets the key of the JWT.
        /// </summary>
        public string? Key { get; set; }

        #endregion

        #region Internal Methods

        internal SymmetricSecurityKey GetSymmetricSecurityKey()
            => new(Encoding.UTF8.GetBytes(Key ?? throw new MissingConfigurationException("No key defined for access token configuration.")));

        #endregion
    }
}
