using CarCostCalculator_App.CCL.AspNetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarCostCalculator_App.CCL.AspNetCore.Authentication
{
    /// <summary>
    /// Provides extension methods to create an endpoint for JWT retrieval.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds an endpoint that returns a JWT to the <see cref="IEndpointRouteBuilder"/>.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <param name="endpointPattern">The endpoint route pattern.</param>
        /// <param name="primaryRole">Security identifier of the primary group.</param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/>.</returns>
        public static IEndpointRouteBuilder RegisterJwtGeneratorEndpoint(this IEndpointRouteBuilder endpoints,
                                                                         [StringSyntax("Route")] string endpointPattern = "Token/JWT",
                                                                         string? primaryRole = null)
        {
            var config = endpoints.ServiceProvider.GetRequiredService<IOptions<JwtConfiguration>>();

            endpoints.MapGet(endpointPattern, (string? subject) => CreateAccessToken(subject, primaryRole, config.Value))
                     .Produces(StatusCodes.Status200OK)
                     .ProducesProblem(StatusCodes.Status400BadRequest)
                     .WithName(nameof(CreateAccessToken))
                     .WithSummary("JWT retrieval")
                     .WithTags(nameof(Authentication))
                     .WithOpenApi();

            return endpoints;
        }

        #endregion

        #region Private Methods

        private static IResult CreateAccessToken(string? subject, string? primaryRole, JwtConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(subject))
            {
                var key = configuration.GetSymmetricSecurityKey();
                var signedCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var claims = new List<Claim>
            {
                new (ClaimTypes.Name, $"JWT\\{subject}"),
                new (JwtRegisteredClaimNames.Sub, subject),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                if (primaryRole is not null)
                {
                    claims.Add(new(ClaimTypes.Role, primaryRole));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Issuer = configuration.Issuer,
                    Audience = configuration.Audience,
                    SigningCredentials = signedCredential,
                    IssuedAt = DateTime.Now,
                    NotBefore = DateTime.Now,
                    Expires = DateTime.Today.AddMonths(1)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenJwt = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(tokenJwt);

                return Results.Ok(token);
            }
            else
            {
                return Results.BadRequest("A valid subject is required to retrieve a new token.");
            }
        }

        #endregion
    }
}
