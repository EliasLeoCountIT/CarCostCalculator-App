using CarCostCalculator_App.CCL.CQRS.HTTP.Models;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Contracts
{
    /// <summary>
    /// Generates Command/Query feature endpoints.
    /// </summary>
    public interface ICommandQueryFeatureEndpointGenerator
    {
        #region Public Methods

        /// <summary>
        /// Tries to generate a feature endpoint for the given request type.
        /// </summary>
        /// <param name="configurator">The <see cref="IRouteConfigurator"/> to be used.</param>
        /// <param name="endpoint">The <see cref="CommandQueryEndpoint"/> to be generated.</param>
        /// <param name="requestType">The actual type of the <see cref="IWebRequest"/>.</param>
        /// <returns>True, if this feature handled the endpoint.</returns>
        bool TryGenerateFeatureEndpoint(IRouteConfigurator configurator, CommandQueryEndpoint endpoint, Type requestType);

        #endregion
    }
}
