using CarCostCalculator_App.CCL.Common.Faults;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup;
using System.Net.Http.Json;
using System.Net.Mime;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Handling
{
    /// <summary>
    /// Defines the base class for handling web requests.
    /// </summary>
    /// <typeparam name="TRequest">The type of request being handled.</typeparam>
    /// <param name="clientProvider"><see cref="HttpClientProvider{TRequest}"/></param>
    /// <param name="requestRouter"><see cref="WebRequestRouter{TRequest}"/></param>
    public abstract class WebRequestHandlerBase<TRequest>(HttpClientProvider<TRequest> clientProvider, WebRequestRouter<TRequest> requestRouter)
        where TRequest : IWebRequest
    {
        #region Private Members

        private readonly HttpClientProvider<TRequest> _clientProvider = clientProvider;
        private readonly WebRequestRouter<TRequest> _requestRouter = requestRouter;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sends the HTTP request and returns the HTTP response message.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the HTTP response message.</returns>
        protected async Task<HttpResponseMessage> SendRequest(TRequest request, CancellationToken cancellationToken)
        {
            using var requestMessage = _requestRouter.EstablishRequestMessage(request);
            using var httpClient = _clientProvider.CreateClient();

            var responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken);

            if (responseMessage.Content.Headers.ContentType?.MediaType?.Equals(MediaTypeNames.Application.ProblemJson, StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                var problemDetails = await responseMessage.Content.ReadFromJsonAsync<RemoteProblemException.ProblemDetails>(cancellationToken);

                if (problemDetails is not null)
                {
                    throw new RemoteProblemException(problemDetails);
                }
            }

            responseMessage.EnsureSuccessStatusCode();

            return responseMessage;
        }

        #endregion
    }
}
