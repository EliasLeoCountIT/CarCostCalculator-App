using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.Utilities;
using CarCostCalculator_App.CCL.CQRS.HTTP;
using CarCostCalculator_App.CCL.CQRS.HTTP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing
{
    /// <summary>
    /// Provides methods to build handlers for Command/Query endpoints.
    /// </summary>
    public static class WebRequestHandlerBuilder
    {
        #region Public Methods

        /// <summary>
        /// Builds a handler for a common Command/Query endpoint.
        /// </summary>
        /// <typeparam name="TRequest">The type of the <see cref="IWebRequest"/>.</typeparam>
        /// <param name="endpoint"><see cref="CommandQueryEndpoint"/></param>
        /// <returns>The <see cref="Delegate"/> executed when the endpoint is matched.</returns>
        public static Delegate BuildCommonEndpointHandler<TRequest>(CommandQueryEndpoint endpoint)
            where TRequest : IWebRequest
        {
            // Uses the request body if the HttpMethod requires it
            return endpoint.HttpMethod.UseRequestBody()
                ? (async ([FromBody] TRequest request, WebRequestProcessor processor, CancellationToken cancellationToken)
                    => await processor.HandleCommonRequest(request, cancellationToken))
                : (async ([AsParameters] TRequest request, WebRequestProcessor processor, CancellationToken cancellationToken)
                    => await processor.HandleCommonRequest(request, cancellationToken));
        }

        /// <summary>
        /// Builds a handler for a Command endpoint that accepts a file.
        /// </summary>
        /// <typeparam name="TCommand">The type of the <see cref="IFileCommandBase"/>.</typeparam>
        /// <returns>The <see cref="Delegate"/> executed when the endpoint is matched.</returns>
        public static Delegate BuildFileCommandEndpointHandler<TCommand>()
            where TCommand : IFileCommandBase
                => async ([FromForm] IFormFile file, [AsParameters] TCommand command, WebRequestProcessor processor, CancellationToken cancellationToken) =>
                {
                    command.ApplyFileItem(file.MapToFileCommandItem());

                    return await processor.HandleCommonRequest(command, cancellationToken);
                };

        /// <summary>
        /// Builds a handler for a <see cref="IFileQuery"/> endpoint.
        /// </summary>
        /// <typeparam name="TQuery">The type of the <see cref="IFileQuery"/>.</typeparam>
        /// <typeparam name="TResponse">The type of response from the endpoint.</typeparam>
        /// <returns>The <see cref="Delegate"/> executed when the endpoint is matched.</returns>
        public static Delegate BuildFileQueryEndpointHandler<TQuery, TResponse>()
            where TQuery : IFileQuery, IQuery<TResponse>
                => async ([AsParameters] TQuery query, WebRequestProcessor processor, CancellationToken cancellationToken)
                    => await processor.HandleFileQuery(query, cancellationToken);

        /// <summary>
        /// Builds a handler for a <see cref="IODataQuery{TItem}"/> endpoint.
        /// </summary>
        /// <typeparam name="TQuery">The type of the <see cref="IODataQuery{TItem}"/>.</typeparam>
        /// <typeparam name="TItem">The type of the requested items.</typeparam>
        /// <returns>The <see cref="Delegate"/> executed when the endpoint is matched.</returns>
        public static Delegate BuildODataQueryEndpointHandler<TQuery, TItem>()
            where TQuery : IODataQuery<TItem>
                => async ([AsParameters] TQuery query, HttpRequest request, WebRequestProcessor processor, CancellationToken cancellationToken)
                     => await processor.HandleODataQuery(query, request, cancellationToken);

        #endregion
    }
}
