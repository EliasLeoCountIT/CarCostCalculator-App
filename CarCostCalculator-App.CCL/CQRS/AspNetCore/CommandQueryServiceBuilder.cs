using CarCostCalculator_App.CCL.Common;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.OData;
using CarCostCalculator_App.CCL.CQRS.HTTP;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.ModelBuilder;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore
{
    /// <inheridoc />
    public class CommandQueryServiceBuilder : CommandQueryServiceBuilderBase
    {
        #region Internal Constructors

        /// <inheritdoc />
        internal CommandQueryServiceBuilder(IServiceCollection services, Action<CommandQueryConfiguration> configure) : base(services, configure) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds essential services for OData endpoints.
        /// </summary>
        /// <returns><see cref="ODataServiceBuilder"/></returns>
        public ODataServiceBuilder AddODataEndpoints()
        {
            var dataQueryInterface = typeof(IODataQuery<>);
            var handlerInterfaceType = typeof(IRequestHandler<,>);
            var handlerType = typeof(ODataResultRequestHandler<>);
            var requestType = typeof(ODataResultRequest<>);
            var resultType = typeof(PagedDataResult<>);

            var registeredTypes = new List<Type>();
            var modelBuilder = new ODataConventionModelBuilder();

            foreach (var oDataType in Configuration.ContractsAssembly
                                                   .GetTypes()
                                                   .Where(t => t.IsSpecificGenericType(dataQueryInterface)))
            {
                //checks if type implements IODataQuery<T> for response type handling
                var requestInterface = oDataType.GetInterface(dataQueryInterface.Name);

                if (requestInterface is not null)
                {
                    //retrieve response type T of IODataQuery<T>
                    var responseType = requestInterface.GetGenericArguments()[0];

                    //add response type to model
                    modelBuilder.AddComplexType(responseType);
                    registeredTypes.Add(responseType);

                    //make generic handler types
                    var concreteInterfaceType = handlerInterfaceType.MakeGenericType(requestType.MakeGenericType(responseType), resultType.MakeGenericType(responseType));
                    var concreteHandlerType = handlerType.MakeGenericType(responseType);

                    //add response type result request handler
                    Services.AddScoped(concreteInterfaceType, concreteHandlerType);
                }
            }

            var edmModel = modelBuilder.GetEdmModel();
            Services.AddSingleton(edmModel);

            return new ODataServiceBuilder(Services, registeredTypes);
        }

        #endregion
    }
}
