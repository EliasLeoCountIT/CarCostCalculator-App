using CarCostCalculator_App.CCL.CQRS.HTTP.Models;
using Microsoft.AspNetCore.Builder;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Contracts
{
    /// <summary>
    /// Configures Command/Query endpoint routes.
    /// </summary>
    public interface IRouteConfigurator
    {
        #region Public Methods

        /// <summary>
        /// Configures a Command/Query endpoint route with a binary response.
        /// </summary>
        /// <param name="endpoint"><see cref="CommandQueryEndpoint"/></param>
        /// <param name="endpointHandler">The <see cref="Delegate"/> executed when the endpoint is matched.</param>
        /// <returns><see cref="IEndpointConventionBuilder"/></returns>
        IEndpointConventionBuilder ConfigureBinaryEndpointRoute(CommandQueryEndpoint endpoint, Delegate endpointHandler);

        /// <summary>
        /// Configures a responding Command/Query endpoint route.
        /// </summary>
        /// <typeparam name="TResponse">The type of response from the endpoint.</typeparam>
        /// <param name="endpoint"><see cref="CommandQueryEndpoint"/></param>
        /// <param name="endpointHandler">The <see cref="Delegate"/> executed when the endpoint is matched.</param>
        /// <returns><see cref="IEndpointConventionBuilder"/></returns>
        IEndpointConventionBuilder ConfigureEndpointRoute<TResponse>(CommandQueryEndpoint endpoint, Delegate endpointHandler);

        /// <summary>
        /// Configures a none responding Command/Query endpoint route.
        /// </summary>
        /// <param name="endpoint"><see cref="CommandQueryEndpoint"/></param>
        /// <param name="endpointHandler">The <see cref="Delegate"/> executed when the endpoint is matched.</param>
        /// <returns><see cref="IEndpointConventionBuilder"/></returns>
        IEndpointConventionBuilder ConfigureEndpointRoute(CommandQueryEndpoint endpoint, Delegate endpointHandler);

        #endregion
    }
}
