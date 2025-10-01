using CarCostCalculator_App.CCL.AspNetCore.Routing;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds essential services for Command/Query endpoints.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <param name="configure">An <see cref="Action{CommandQueryEndpointsConfiguration}"/> to configure the provided <see cref="CommandQueryConfiguration"/>.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static CommandQueryServiceBuilder AddCommandQueryEndpoints(this IServiceCollection services, Action<CommandQueryConfiguration> configure)
        {
            // configure endpoints
            services.AddOptions<CommandQueryConfiguration>().Configure(configure);
            services.AddTransient<CommandQueryEndpointGenerator>();
            services.AddScoped<WebRequestProcessor>();

            // configure exception handling
            services.AddExceptionHandler<WebRequestExceptionHandler>();
            services.AddProblemDetails(cfg => cfg.CustomizeProblemDetails = WebRequestExceptionHandler.CustomizeProblemDetails);

            return new CommandQueryServiceBuilder(services, configure);
        }

        /// <summary>
        /// Configures the route handler to throw an exception on bad request.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureRouteHandlerThrowOnBadRequest(this IServiceCollection services)
            => services.ConfigureOptions<RouteHandlerThrowOnBadRequestConfigurator>();

        #endregion
    }
}
