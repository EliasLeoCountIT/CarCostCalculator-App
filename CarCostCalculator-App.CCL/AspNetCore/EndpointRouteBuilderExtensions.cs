using CarCostCalculator_App.CCL.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.AspNetCore
{
    /// <summary>
    /// Provides extension methods to allow automatic endpoints for Commands and Queries.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds automatic endpoints for Commands and Queries to the <see cref="IEndpointRouteBuilder"/>.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <returns><see cref="IEndpointRouteBuilder"/></returns>
        public static IEndpointRouteBuilder MapCommandQueryEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var generator = endpoints.ServiceProvider.GetRequiredService<CommandQueryEndpointGenerator>();
            var configurator = new MinimalApiRouteConfigurator(endpoints);

            generator.GenerateEndpoints(configurator);

            return endpoints;
        }

        #endregion
    }
}
