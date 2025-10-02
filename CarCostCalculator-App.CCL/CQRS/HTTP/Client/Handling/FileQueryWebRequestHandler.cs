using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using System.Net;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Handling
{
    /// <summary>
    /// Defines a handler for a <see cref="IFileQuery"/>.
    /// </summary>
    /// <typeparam name="TFileQuery">The type of the <see cref="IFileQuery"/>.</typeparam>
    /// <param name="clientProvider"><see cref="HttpClientProvider{IFileQuery}"/></param>
    /// <param name="requestRouter"><see cref="WebRequestRouter{IFileQuery}"/></param>
    public class FileQueryWebRequestHandler<TFileQuery>(HttpClientProvider<TFileQuery> clientProvider, WebRequestRouter<TFileQuery> requestRouter)
        : WebRequestHandlerBase<TFileQuery>(clientProvider, requestRouter), IRequestHandler<TFileQuery, FileQueryItem?>
        where TFileQuery : class, IFileQuery
    {
        #region Public Methods

        /// <summary>
        /// Handles the request for a file query.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning a <see cref="FileQueryItem"/>.</returns>
        public async Task<FileQueryItem?> Handle(TFileQuery request, CancellationToken cancellationToken)
        {
            var responseMessage = await SendRequest(request, cancellationToken);

            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }
            else
            {
                var responseContentStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
                var responseMessageHeaders = responseMessage.Content.Headers;

                var result = new FileQueryItem(responseContentStream,
                    responseMessageHeaders.ContentType?.MediaType ?? throw new InvalidDataException("No media type transmitted."),
                    responseMessageHeaders.ContentDisposition?.FileNameStar,
                    responseMessageHeaders.LastModified);

                return result;
            }
        }

        #endregion
    }
}
