using CarCostCalculator_App.CCL.CQRS.AspNetCore.OData;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.CQRS.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for <see cref="ODataServiceBuilder"/>.
    /// </summary>
    public static class ODataServiceBuilderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds <see cref="EntityFrameworkQueryableExtensions.ToListAsync"/> as behavior to retrieve the OData result.
        /// </summary>
        /// <param name="builder">The <see cref="ODataServiceBuilder"/>.</param>
        /// <returns><see cref="ODataServiceBuilder"/></returns>
        public static ODataServiceBuilder AddAsyncBehavior(this ODataServiceBuilder builder)
        {
            var behaviorInterfaceType = typeof(IPipelineBehavior<,>);
            var behaviorType = typeof(ODataResultAsyncBehavior<>);
            var requestType = typeof(ODataResultRequest<>);
            var resultType = typeof(PagedDataResult<>);

            foreach (var entityType in builder.RegisteredTypes)
            {
                //make generic behavior types
                var concreteInterfaceType = behaviorInterfaceType.MakeGenericType(requestType.MakeGenericType(entityType), resultType.MakeGenericType(entityType));
                var concreteBehaviorType = behaviorType.MakeGenericType(entityType);

                //add response type result request handler
                builder.Services.AddScoped(concreteInterfaceType, concreteBehaviorType);
            }

            return builder;
        }

        #endregion
    }
}
