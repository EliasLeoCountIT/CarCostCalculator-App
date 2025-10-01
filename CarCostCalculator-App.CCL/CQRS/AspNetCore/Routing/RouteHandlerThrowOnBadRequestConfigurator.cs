using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing
{
    internal sealed class RouteHandlerThrowOnBadRequestConfigurator : IPostConfigureOptions<RouteHandlerOptions>
    {
        #region Public Methods

        /// <summary>
        /// Invoked to configure: Throw an exception if the request body is not valid.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configured.</param>
        public void PostConfigure(string? name, RouteHandlerOptions options)
            => options.ThrowOnBadRequest = true;

        #endregion
    }
}
