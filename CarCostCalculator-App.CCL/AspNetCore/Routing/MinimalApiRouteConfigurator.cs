using CarCostCalculator_App.CCL.CQRS.AspNetCore.Contracts;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Net.Mime;

namespace CarCostCalculator_App.CCL.AspNetCore.Routing
{
    /// <summary>
    /// Configures Command/Query endpoint routes using minimal API.
    /// </summary>
    internal class MinimalApiRouteConfigurator(IEndpointRouteBuilder endpoints) : IRouteConfigurator
    {
        #region Private Members

        private readonly IEndpointRouteBuilder _endpoints = endpoints;

        #endregion

        #region Public Methods

        public IEndpointConventionBuilder ConfigureBinaryEndpointRoute(CommandQueryEndpoint endpoint, Delegate endpointHandler)
            => MapEndpoint(endpoint, endpointHandler).ProducesResponseCodes<FileResult>(MediaTypeNames.Application.Octet);

        /// <inheritdoc/>
        public IEndpointConventionBuilder ConfigureEndpointRoute<TResponse>(CommandQueryEndpoint endpoint, Delegate endpointHandler)
            => MapEndpoint(endpoint, endpointHandler).ProducesResponseCodes<TResponse>();

        /// <inheritdoc/>
        public IEndpointConventionBuilder ConfigureEndpointRoute(CommandQueryEndpoint endpoint, Delegate endpointHandler)
            => MapEndpoint(endpoint, endpointHandler).ProducesResponseCodes();

        #endregion

        #region Private Methods

        private RouteHandlerBuilder MapEndpoint(CommandQueryEndpoint endpoint, Delegate endpointHandler)
        {
            var bldr = _endpoints.MapMethods(endpoint.Route, [endpoint.HttpMethod.Method], endpointHandler)
                                 .WithName(endpoint.Name);

            //apply configured metadata
            if (endpoint.Metadata != null)
            {
                if (endpoint.Metadata.Tags.Length != 0)
                {
                    bldr = bldr.WithTags(endpoint.Metadata.Tags);
                }

                if (endpoint.Metadata.Description != null)
                {
                    bldr = bldr.WithDescription(endpoint.Metadata.Description);
                }

                if (endpoint.Metadata.Summary != null)
                {
                    bldr = bldr.WithSummary(endpoint.Metadata.Summary);
                }
            }

            //apply fallback metadata
            if (endpoint.Metadata == null || endpoint.Metadata.Tags.Length == 0)
            {
                bldr = bldr.WithTags(nameof(CQRS));
            }

            return bldr.WithOpenApi();
        }

        #endregion
    }
}
