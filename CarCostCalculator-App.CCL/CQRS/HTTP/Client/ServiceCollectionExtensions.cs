using CarCostCalculator_App.CCL.Common;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Handling;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.OData;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using MediatR.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds CQRS (Command Query Responsibility Segregation) client services for HTTP endpoints.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <param name="configure">An <see cref="Action{CommandQueryHttpClientConfiguration}"/> to configure the provided <see cref="CommandQueryConfiguration"/>.</param>
        /// <returns><see cref="CommandQueryServiceBuilder"/></returns>
        public static CommandQueryServiceBuilder AddCommandQueryClient(this IServiceCollection services, Action<CommandQueryConfiguration> configure)
        {
            var builder = new CommandQueryServiceBuilder(services, configure);
            var handlerType = typeof(DefaultWebRequestHandler<>);
            var handlerInterfaceType = typeof(IRequestHandler<>);
            var respondingHandlerInterfaceType = typeof(IRequestHandler<,>);
            var respondingHandlerType = typeof(DefaultWebRequestHandler<,>);
            var respondingPagedDataHandlerType = typeof(PagedDataWebRequestHandler<,>);
            var fileQueryHandlerType = typeof(FileQueryWebRequestHandler<>);
            var oDataHandlerType = typeof(ODataWebRequestHandler<,>);
            var pagedDataResultType = typeof(PagedDataResult<>);
            var oDataRequestType = typeof(ODataQueryWrapper<>);

            foreach (var requestType in builder.Configuration.GetRequestTypes())
            {
                Type concreteInterfaceType, concreteHandlerType;

                //checks if type implements IRequest<T> for response type handling
                var requestInterface = requestType.GetInterface(typeof(IRequest<>).Name);

                if (requestInterface is not null)
                {
                    //retrieve response type T of IRequest<T>
                    var responseType = requestInterface.GenericTypeArguments[0];

                    //handle underlying response type and wrapper of OData query
                    if (requestType.IsSpecificGenericType(typeof(IODataQuery<>)))
                    {
                        responseType = responseType.GenericTypeArguments[0];

                        //make generic handler types for OData request
                        concreteInterfaceType = respondingHandlerInterfaceType.MakeGenericType(oDataRequestType.MakeGenericType(responseType), pagedDataResultType.MakeGenericType(responseType));
                        concreteHandlerType = oDataHandlerType.MakeGenericType(requestType, responseType);
                    }
                    else if (typeof(IFileQuery).IsAssignableFrom(requestType))
                    {
                        //make generic handler types for file query request
                        concreteInterfaceType = respondingHandlerInterfaceType.MakeGenericType(requestType, responseType);
                        concreteHandlerType = fileQueryHandlerType.MakeGenericType(requestType);
                    }
                    else if (responseType.BaseType == typeof(PagedDataResult))
                    {
                        //make generic handler types for paged data responding request
                        concreteInterfaceType = respondingHandlerInterfaceType.MakeGenericType(requestType, responseType);
                        concreteHandlerType = respondingPagedDataHandlerType.MakeGenericType(requestType, responseType.GenericTypeArguments[0]);
                    }
                    else
                    {
                        //make generic handler types for default responding request
                        concreteInterfaceType = respondingHandlerInterfaceType.MakeGenericType(requestType, responseType);
                        concreteHandlerType = respondingHandlerType.MakeGenericType(requestType, responseType);
                    }
                }
                else
                {
                    //make generic handler types for none responding request
                    concreteInterfaceType = handlerInterfaceType.MakeGenericType(requestType);
                    concreteHandlerType = handlerType.MakeGenericType(requestType);
                }

                //add web request handler
                services.AddScoped(concreteInterfaceType, concreteHandlerType);
            }

            //add web request router and required MediatR services
            services.AddSingleton(typeof(WebRequestRouter<>));
            ServiceRegistrar.AddRequiredServices(services, new());

            //try add HTTP client provider and name resolver, so any existing registration doesn't get overridden
            services.TryAddSingleton(typeof(HttpClientProvider<>));
            services.TryAddSingleton<HttpClientNameResolver>();

            return builder;
        }

        #endregion
    }
}
