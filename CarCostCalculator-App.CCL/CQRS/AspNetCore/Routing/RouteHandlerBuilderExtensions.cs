using CarCostCalculator_App.CCL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing
{
    internal static class RouteHandlerBuilderExtensions
    {
        #region Internal Methods

        internal static RouteHandlerBuilder ProducesResponseCodes<TResponse>(this RouteHandlerBuilder builder, string? mimeType = null)
        {
            builder.Produces<TResponse>(contentType: mimeType)
                   .ProducesDefaultResponseCodes();

            if (typeof(TResponse).IsAssignableTo(typeof(IEnumerable))
                || typeof(TResponse).IsAssignableTo(typeof(PagedDataResult)))
            {
                builder.Produces(StatusCodes.Status204NoContent);
            }

            return builder;
        }

        internal static RouteHandlerBuilder ProducesResponseCodes(this RouteHandlerBuilder builder)
            => builder.Produces(StatusCodes.Status200OK)
                      .ProducesDefaultResponseCodes();

        #endregion

        #region Private Methods

        private static RouteHandlerBuilder ProducesDefaultResponseCodes(this RouteHandlerBuilder builder)
            => builder.ProducesProblem(StatusCodes.Status400BadRequest)
                      .ProducesProblem(StatusCodes.Status401Unauthorized)
                      .ProducesProblem(StatusCodes.Status403Forbidden)
                      .ProducesProblem(StatusCodes.Status404NotFound)
                      .ProducesProblem(StatusCodes.Status409Conflict)
                      .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);

        #endregion
    }
}
