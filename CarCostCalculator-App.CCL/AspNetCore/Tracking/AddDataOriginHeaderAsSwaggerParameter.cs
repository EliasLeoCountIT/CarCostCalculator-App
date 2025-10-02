using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CarCostCalculator_App.CCL.AspNetCore.Tracking
{
    /// <summary>
    /// Used to add the data origin header as parameter in the swagger UI.
    /// </summary>
    /// <param name="dataOriginHeader">Data origin header key.</param>
    public class AddDataOriginHeaderAsSwaggerParameter(string dataOriginHeader) : IOperationFilter
    {
        #region Private Members

        private readonly string _dataOriginHeader = dataOriginHeader;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the data origin header as parameter to every operation in the swagger UI.
        /// </summary>
        /// <param name="operation">The created OpenApi operation</param>
        /// <param name="context">The context of the created OpenApi operation</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= [];

            if (context.ApiDescription.HttpMethod != "GET" &&
                 !operation.Parameters.Any(p => p.Name == _dataOriginHeader))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = _dataOriginHeader,
                    In = ParameterLocation.Header,
                    Description = "The data origin of the request.",
                    Required = false,// "not required" to be able to test the fallback
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    Example = new OpenApiString("Swagger")
                });
            }
        }

        #endregion
    }
}
