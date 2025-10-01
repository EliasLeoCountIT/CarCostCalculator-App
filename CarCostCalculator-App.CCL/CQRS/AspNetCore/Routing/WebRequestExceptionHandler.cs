using CarCostCalculator_App.CCL.Common.Faults;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing
{
    /// <summary>
    /// Handling of exceptions in CQRS ASP.NET Core applications.
    /// </summary>
    /// <param name="logger">The <see cref="HttpContext"/> for the request.</param>
    /// <param name="problemDetailsService">A type that provide functionality to create a <see cref="ProblemDetails"/> response.</param>
    public class WebRequestExceptionHandler(ILogger<WebRequestExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        #region Private Members

        private readonly ILogger<WebRequestExceptionHandler> _logger = logger;
        private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

        #endregion

        #region Public Methods

        /// <summary>
        /// Tries to handle the specified exception asynchronously within the ASP.NET Core pipeline.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/> for the request.</param>
        /// <param name="exception">The current exception.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task that represents the asynchronous read operation.
        /// <see langword="true"/> if the exception was handled successfully; otherwise <see langword="false"/>.
        /// </returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var context = new ProblemDetailsContext
            {
                ProblemDetails = BuildProblemDetails(exception),
                HttpContext = httpContext,
                Exception = exception
            };

            _logger.LogError(exception, "An error occurred while processing the request: {Problem}", context.ProblemDetails.Title);

            httpContext.Response.StatusCode = context.ProblemDetails.Status ?? StatusCodes.Status500InternalServerError;

            return await _problemDetailsService.TryWriteAsync(context);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// This operation customizes the current <see cref="ProblemDetailsContext"/> instance.
        /// </summary>
        /// <param name="context">The current <see cref="ProblemDetailsContext"/> instance.</param>
        internal static void CustomizeProblemDetails(ProblemDetailsContext context)
        {
            context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

            context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

            var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

            if (activity is not null)
            {
                context.ProblemDetails.Extensions.TryAdd("traceId", activity.Id);
            }
        }

        #endregion

        #region Private Methods

        private static ProblemDetails BuildProblemDetails(Exception exception)
        {
            var pd = new ProblemDetails
            {
                Detail = exception.Message
            };

            var additionalInformation = ParseInnerExceptions(exception).ToList();

            if (additionalInformation.Count != 0)
            {
                pd.Extensions.Add(nameof(additionalInformation), additionalInformation);
            }

            (pd.Title, pd.Status) = exception switch
            {
                ValidationProblemException vpex => (vpex.Error, StatusCodes.Status422UnprocessableEntity),
                ProblemException pex => (pex.Error, StatusCodes.Status400BadRequest),
                BadHttpRequestException => ("Bad HTTP Request", StatusCodes.Status400BadRequest),
                UnauthorizedAccessException => ("Insufficient Permissions", StatusCodes.Status403Forbidden),
                AuthenticationException => ("No Authentication", StatusCodes.Status401Unauthorized),
                KeyNotFoundException or FileNotFoundException => ("Not Found", StatusCodes.Status404NotFound),
                TimeoutException => ("Request Timeout", StatusCodes.Status408RequestTimeout),
                ArgumentException => ("Invalid Arguments", StatusCodes.Status409Conflict),
                ApplicationException => ("Invalid Operation", StatusCodes.Status400BadRequest),
                MissingConfigurationException => ("Configuration Error", StatusCodes.Status500InternalServerError),
                DbUpdateConcurrencyException => ("Concurrency Conflict", StatusCodes.Status409Conflict),
                _ => ("Unexpected Error", StatusCodes.Status500InternalServerError)
            };

            // Add validation errors to the response.
            if (exception is ValidationProblemException validationProblemException)
            {
                pd.Extensions.Add(nameof(validationProblemException.Errors), validationProblemException.Errors);
            }

            return pd;
        }

        private static IEnumerable<string> ParseInnerExceptions(Exception exception)
        {
            if (exception.InnerException != null)
            {
                yield return exception.InnerException.Message;

                foreach (var innerMessage in ParseInnerExceptions(exception.InnerException))
                {
                    yield return innerMessage;
                }
            }
        }

        #endregion
    }
}
