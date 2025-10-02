using CarCostCalculator_App.CCL.CQRS.Abstractions;
using MediatR;

namespace CarCostCalculator_App.CCL.CQRS.Authorization
{
    /// <summary>
    /// Authorization behavior for <see cref="MediatR"/> requests.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request to authorize.</typeparam>
    /// <typeparam name="TResponse">Response type of the request.</typeparam>
    /// <param name="authorizationService">User role validation service.</param>
    public class AuthorizationBehavior<TRequest, TResponse>(AuthorizationService authorizationService) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        #region Private Members

        private readonly AuthorizationService _authorizationService = authorizationService;

        #endregion

        #region Public Methods

        /// <summary>
        /// Authorization pipeline handler.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TResponse"/>.</returns>
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(next);

            var authorizeAttributes = Attribute.GetCustomAttributes(request.GetType(), typeof(AuthorizeAttribute)).Cast<AuthorizeAttribute>().ToList();

            if (authorizeAttributes.Count != 0)
            {
                var isAuthorized = _authorizationService.IsAuthorized(authorizeAttributes.Select(attribute => attribute.Roles));

                return isAuthorized
                        ? next(cancellationToken)
                        : throw new UnauthorizedAccessException("User doesn't have all the required roles to perform this operation!");
            }
            else
            {
                //there is no authorize attribute on the given request
                return next(cancellationToken);
            }
        }

        #endregion
    }
}
