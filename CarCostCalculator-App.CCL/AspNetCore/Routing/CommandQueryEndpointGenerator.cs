using CarCostCalculator_App.CCL.Common;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.Contracts;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP;
using CarCostCalculator_App.CCL.CQRS.HTTP.Models;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace CarCostCalculator_App.CCL.AspNetCore.Routing
{
    /// <summary>
    /// Provides Command/Query endpoint generation.
    /// </summary>
    /// <param name="options">The <see cref="IOptions{CommandQueryConfiguration}"/> to be used.</param>
    /// <param name="features">Additional features to be used.</param>
    public class CommandQueryEndpointGenerator(IOptions<CommandQueryConfiguration> options, IEnumerable<ICommandQueryFeatureEndpointGenerator> features)
    {
        #region Private Members

        private readonly IEnumerable<ICommandQueryFeatureEndpointGenerator> _features = features;
        private readonly IOptions<CommandQueryConfiguration> _options = options;

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates endpoints for Commands and Queries.
        /// </summary>
        /// <param name="configurator">The <see cref="IRouteConfigurator"/> to be used.</param>
        public void GenerateEndpoints(IRouteConfigurator configurator)
        {
            foreach (var requestType in _options.Value.GetRequestTypes())
            {
                //resolve endpoint for request type
                var endpoint = CommandQueryEndpointResolver.ResolveEndpoint(requestType);

                //check additional features for endpoint generation
                if (!_features.Any(f => f.TryGenerateFeatureEndpoint(configurator, endpoint, requestType)))
                {
                    //if no feature has handled the endpoint, generate common endpoint for the request type
                    GenerateCommonEndpoint(configurator, endpoint, requestType);
                }
            }
        }

        #endregion

        #region Private Methods

        private static void AddCommonNoneRespondingEndpoint<TRequest>(IRouteConfigurator configurator, CommandQueryEndpoint endpoint)
            where TRequest : IWebRequest
        {
            var endpointHandler = WebRequestHandlerBuilder.BuildCommonEndpointHandler<TRequest>(endpoint);

            configurator.ConfigureEndpointRoute(endpoint, endpointHandler);
        }

        private static void AddCommonRespondingEndpoint<TRequest, TResponse>(IRouteConfigurator configurator, CommandQueryEndpoint endpoint)
            where TRequest : IWebRequest, IRequest<TResponse>
        {
            var endpointHandler = WebRequestHandlerBuilder.BuildCommonEndpointHandler<TRequest>(endpoint);

            configurator.ConfigureEndpointRoute<TResponse>(endpoint, endpointHandler);
        }

        private static void AddFileCommandEndpoint<TCommand>(IRouteConfigurator configurator, CommandQueryEndpoint endpoint)
            where TCommand : IFileCommand
        {
            var endpointHandler = WebRequestHandlerBuilder.BuildFileCommandEndpointHandler<TCommand>();

            configurator.ConfigureEndpointRoute(endpoint, endpointHandler)
                        .DisableAntiforgery();
        }

        private static void AddFileCommandRespondingEndpoint<TCommand, TResponse>(IRouteConfigurator configurator, CommandQueryEndpoint endpoint)
            where TCommand : IFileCommand<TResponse>
        {
            var endpointHandler = WebRequestHandlerBuilder.BuildFileCommandEndpointHandler<TCommand>();

            configurator.ConfigureEndpointRoute<TResponse>(endpoint, endpointHandler)
                        .DisableAntiforgery();
        }

        private static void AddFileQueryEndpoint<TQuery, TResponse>(IRouteConfigurator configurator, CommandQueryEndpoint endpoint)
            where TQuery : IFileQuery, IQuery<TResponse>
        {
            var endpointHandler = WebRequestHandlerBuilder.BuildFileQueryEndpointHandler<TQuery, TResponse>();

            configurator.ConfigureBinaryEndpointRoute(endpoint, endpointHandler);
        }

        private static void AddODataQueryEndpoint<TQuery, TItem>(IRouteConfigurator configurator, CommandQueryEndpoint endpoint)
                where TQuery : IODataQuery<TItem>
        {
            var endpointHandler = WebRequestHandlerBuilder.BuildODataQueryEndpointHandler<TQuery, TItem>();

            configurator.ConfigureEndpointRoute<PagedDataResult<TItem>>(endpoint, endpointHandler);
        }

        private static string RetrieveEndpointMethodName(Type requestType)
        {
            if (requestType.IsSpecificGenericType(typeof(IODataQuery<>)))
            {
                return nameof(AddODataQueryEndpoint);
            }
            else if (typeof(IFileQuery).IsAssignableFrom(requestType))
            {
                return nameof(AddFileQueryEndpoint);
            }
            else if (typeof(IFileCommand).IsAssignableFrom(requestType))
            {
                return nameof(AddFileCommandEndpoint);
            }
            else if (requestType.IsSpecificGenericType(typeof(IFileCommand<>)))
            {
                return nameof(AddFileCommandRespondingEndpoint);
            }
            else if (typeof(ICommand).IsAssignableFrom(requestType))
            {
                return nameof(AddCommonNoneRespondingEndpoint);
            }
            else
            {
                return nameof(AddCommonRespondingEndpoint);
            }
        }

        private void GenerateCommonEndpoint(IRouteConfigurator configurator, CommandQueryEndpoint endpoint, Type requestType)
        {
            //checks if type implements IRequest<T> for response type handling
            var requestInterface = requestType.GetInterface(typeof(IRequest<>).Name);

            //initialize endpoint mapping method
            var mapMethod = GetType().GetMethod(RetrieveEndpointMethodName(requestType), BindingFlags.NonPublic | BindingFlags.Static);

            if (requestInterface != null)
            {
                //retrieve response type T of IRequest<T>
                var responseType = requestInterface.GetGenericArguments()[0];

                //handle underlying response type of OData query
                if (requestType.IsSpecificGenericType(typeof(IODataQuery<>)))
                {
                    responseType = responseType.GetGenericArguments()[0];
                }

                //establish query or command with response endpoint
                mapMethod = mapMethod?.MakeGenericMethod(requestType, responseType);
            }
            else if (typeof(ICommand).IsAssignableFrom(requestType))
            {
                //establish command without response endpoint
                mapMethod = mapMethod?.MakeGenericMethod(requestType);
            }

            //invoke endpoint mapping method
            mapMethod?.Invoke(null, [configurator, endpoint]);
        }

        #endregion
    }
}
