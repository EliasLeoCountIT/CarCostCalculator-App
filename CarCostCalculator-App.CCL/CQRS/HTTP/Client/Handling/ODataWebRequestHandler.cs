using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.OData;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using System.Net;
using System.Net.Http.Json;


namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Handling
{
    /// <summary>
    /// Defines a handler for a <see cref="IODataQuery{TItem}"/>.
    /// </summary>
    /// <typeparam name="TODataQuery">The type of the <see cref="IODataQuery{TItem}"/>.</typeparam>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    /// <param name="clientProvider"><see cref="HttpClientProvider{TODataQuery}"/></param>
    /// <param name="requestRouter"><see cref="WebRequestRouter{TODataQuery}"/></param>
    public class ODataWebRequestHandler<TODataQuery, TItem>(HttpClientProvider<TODataQuery> clientProvider, WebRequestRouter<TODataQuery> requestRouter)
        : WebRequestHandlerBase<TODataQuery>(clientProvider, requestRouter), IRequestHandler<ODataQueryWrapper<TItem>, PagedDataResult<TItem>>
        where TODataQuery : class, IODataQuery<TItem>
    {
        #region Public Methods

        /// <summary>
        /// Handles the request for a OData query.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning a <see cref="PagedDataResult{TItem}"/>.</returns>
        public async Task<PagedDataResult<TItem>> Handle(ODataQueryWrapper<TItem> request, CancellationToken cancellationToken)
        {
            var query = request.Query as TODataQuery ?? throw new InvalidOperationException("Invalid OData query type.");

            var responseMessage = await SendRequest(query, cancellationToken);

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
