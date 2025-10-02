using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using System.Net;
using System.Net.Http.Json;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Handling
{
    /// <summary>
    /// Defines a handler for a <see cref="IWebRequest"/> with <see cref="PagedDataResult"/> response.
    /// </summary>
    /// <typeparam name="TRequest">The type of request being handled.</typeparam>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    /// <param name="clientProvider"><see cref="HttpClientProvider{TRequest}"/></param>
    /// <param name="requestRouter"><see cref="WebRequestRouter{TRequest}"/></param>
    public class PagedDataWebRequestHandler<TRequest, TItem>(HttpClientProvider<TRequest> clientProvider, WebRequestRouter<TRequest> requestRouter)
        : WebRequestHandlerBase<TRequest>(clientProvider, requestRouter), IRequestHandler<TRequest, PagedDataResult<TItem>>
        where TRequest : IRequest<PagedDataResult<TItem>>, IWebRequest
    {
        #region Public Methods

        /// <summary>
        /// Handles the request and returns the response.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning a <see cref="PagedDataResult{TItem}"/>.</returns>
        public async Task<PagedDataResult<TItem>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var responseMessage = await SendRequest(request, cancellationToken);

            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return PagedDataResult<TItem>.Empty;
            }
            else
            {
                return await responseMessage.Content.ReadFromJsonAsync<PagedDataResult<TItem>>(cancellationToken) ?? PagedDataResult<TItem>.Empty;
            }
        }

        #endregion
    }
}
