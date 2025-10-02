using CarCostCalculator_App.CCL.CQRS.Authorization;
using CarCostCalculator_App.CCL.CQRS.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.CQRS
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds essential services for command and query authorization.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddRequestAuthorization(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddScoped<AuthorizationService>();

            return services;
        }

        /// <summary>
        /// Adds essential services for the <see cref="RequestContext"/>.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddRequestContext(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestContextTraceBehavior<,>));
            services.AddScoped<IRequestContextIdProvider, RequestContextIdProvider>();
            services.AddScoped<IRequestContext, RequestContext>();

            return services;
        }

        #endregion
    }
}
