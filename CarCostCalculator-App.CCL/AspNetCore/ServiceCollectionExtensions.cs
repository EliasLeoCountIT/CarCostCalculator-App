using CarCostCalculator_App.CCL.AspNetCore.Authentication;
using CarCostCalculator_App.CCL.AspNetCore.Configuration;
using CarCostCalculator_App.CCL.Common;
using CarCostCalculator_App.CCL.Common.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace CarCostCalculator_App.CCL.AspNetCore
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds essential services for <see cref="IUserProvider"/> based on <see cref="IHttpContextAccessor"/>.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddHttpContextAuthenticationAccessor(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationAccessor, HttpContextAuthenticationAccessor>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="SwaggerAuthorizationMiddlewareConfiguration"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <param name="requiredRole">Required role for Swagger UI authorization.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddSwaggerAuthorizationMiddleware(this IServiceCollection services, string requiredRole)
        {
            services.AddOptions<SwaggerAuthorizationMiddlewareConfiguration>().Configure(opt => opt.RequiredRole = requiredRole);

            return services;
        }

        /// <summary>
        /// Configure enum string converter for the endpoints.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureJsonOptions(this IServiceCollection services)
        {
            //has to be configured twice until this issue is resolved
            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2293
            services.ConfigureHttpJsonOptions(opt => opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                    .Configure<JsonOptions>(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                    .Configure<JsonOptions>(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            return services;
        }

        /// <summary>
        /// Adds available windows authentication groups to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <param name="authorizationGroupsType">The type that contains the authorization group constants.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection ConfigureWindowsAuthenticationGroups(this IServiceCollection services, Type authorizationGroupsType)
            => services.Configure<WindowsAuthenticationSettings>(opt => opt.AvailableGroups = authorizationGroupsType.RetrieveConstants());

        #endregion
    }
}
