
using CarCostCalculator_App.CCL.Common;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Models;
using CarCostCalculator_App.CCL.Metadata;
using System.Reflection;

namespace CarCostCalculator_App.CCL.CQRS.HTTP
{
    /// <summary>
    /// Resolves associated HTTP endpoints for specified request types.
    /// </summary>
    public static class CommandQueryEndpointResolver
    {
        #region Public Methods

        /// <summary>
        /// Resolves the <see cref="CommandQueryEndpoint"/> for the specified request type.
        /// </summary>
        /// <param name="requestType">The <see cref="IWebRequest"/> type.</param>
        /// <returns><see cref="CommandQueryEndpoint"/></returns>
        public static CommandQueryEndpoint ResolveEndpoint(Type requestType)
            => new(requestType.Name,
                   ResolveEndpointRoute(requestType),
                   ResolveHttpMethod(requestType),
                   requestType.GetCustomAttribute<MetadataAttribute>());

        /// <summary>
        /// Resolves the endpoint route for the specified request type.
        /// </summary>
        /// <param name="requestType">The <see cref="IWebRequest"/> type.</param>
        /// <returns>The route of the endpoint.</returns>
        public static string ResolveEndpointRoute(Type requestType)
        {
            var endpointAttribute = requestType.GetCustomAttribute<EndpointAttribute>();

            return endpointAttribute?.Route ?? requestType.Name;
        }

        /// <summary>
        /// Resolves the HTTP method for the specified request type.
        /// </summary>
        /// <param name="requestType">The <see cref="IWebRequest"/> type.</param>
        /// <returns><see cref="HttpMethod"/></returns>
        /// <exception cref="NotImplementedException">Unable to resolve HTTP method for the specified request type.</exception>
        public static HttpMethod ResolveHttpMethod(Type requestType)
        {
            if (requestType.IsSpecificGenericType(typeof(IQuery<>)))
            {
                return HttpMethod.Get;
            }
            else if (typeof(IDeleteCommand).IsAssignableFrom(requestType) || requestType.IsSpecificGenericType(typeof(IDeleteCommand<>)))
            {
                return HttpMethod.Delete;
            }
            else if (typeof(IIdempotentCommand).IsAssignableFrom(requestType) || requestType.IsSpecificGenericType(typeof(IIdempotentCommand<>)))
            {
                return HttpMethod.Put;
            }
            else if (typeof(ICommand).IsAssignableFrom(requestType) || requestType.IsSpecificGenericType(typeof(ICommand<>)))
            {
                return HttpMethod.Post;
            }
            else
            {
                throw new NotImplementedException($"Unable to resolve HTTP method for request type '{requestType.FullName}'.");
            }
        }

        #endregion
    }
}
