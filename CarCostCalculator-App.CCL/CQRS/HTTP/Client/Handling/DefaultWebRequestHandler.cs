using CarCostCalculator_App.CCL.Common;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup;
using MediatR;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Handling
{
    /// <summary>
    /// Defines a handler for a none responding <see cref="IWebRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of request being handled.</typeparam>
    /// <param name="clientProvider"><see cref="HttpClientProvider{TRequest}"/></param>
    /// <param name="requestRouter"><see cref="WebRequestRouter{TRequest}"/></param>
    public class DefaultWebRequestHandler<TRequest>(HttpClientProvider<TRequest> clientProvider, WebRequestRouter<TRequest> requestRouter)
        : WebRequestHandlerBase<TRequest>(clientProvider, requestRouter), IRequestHandler<TRequest>
        where TRequest : IRequest, IWebRequest
    {
        #region Public Methods

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task.</returns>
        public Task Handle(TRequest request, CancellationToken cancellationToken) => SendRequest(request, cancellationToken);

        #endregion
    }

    /// <summary>
    /// Defines a handler for a responding <see cref="IWebRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of request being handled.</typeparam>
    /// <typeparam name="TResponse">The type of response from the handler.</typeparam>
    /// <param name="clientProvider"><see cref="HttpClientProvider{TRequest}"/></param>
    /// <param name="requestRouter"><see cref="WebRequestRouter{TRequest}"/></param>
    public class DefaultWebRequestHandler<TRequest, TResponse>(HttpClientProvider<TRequest> clientProvider, WebRequestRouter<TRequest> requestRouter)
        : WebRequestHandlerBase<TRequest>(clientProvider, requestRouter), IRequestHandler<TRequest, TResponse?>
        where TRequest : IRequest<TResponse>, IWebRequest
    {
        #region Public Methods

        /// <summary>
        /// Handles the request and returns the response.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <typeparamref name="TResponse"/>.</returns>
        public async Task<TResponse?> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var responseMessage = await SendRequest(request, cancellationToken);

            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                if (typeof(TResponse).IsEnumerableType())
                {
                    // Return a empty collection for enumerable types.
                    return JsonSerializer.Deserialize<TResponse>("[]");
                }
                else
                {
                    // Return the default value for non-enumerable types.
                    return default;
                }
            }

            return await responseMessage.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        }

        #endregion
    }
}
