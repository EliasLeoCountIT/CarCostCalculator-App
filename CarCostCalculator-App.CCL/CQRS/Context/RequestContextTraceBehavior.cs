using MediatR;

namespace CarCostCalculator_App.CCL.CQRS.Context
{
    /// <summary>
    /// Trace behavior for <see cref="MediatR"/> requests.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request to authorize.</typeparam>
    /// <typeparam name="TResponse">Response type of the request.</typeparam>
    /// <param name="requestContext">Current request context.</param>
    public class RequestContextTraceBehavior<TRequest, TResponse>(IRequestContext requestContext) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        #region Private Members

        private readonly RequestContext? _requestContext = requestContext as RequestContext;

        #endregion

        #region Public Methods

        /// <summary>
        /// Request trace pipeline handler.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TResponse"/>.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _requestContext?.Push(request);
            var response = await next(cancellationToken);
            _requestContext?.Pop();

            return response;
        }

        #endregion
    }
}
