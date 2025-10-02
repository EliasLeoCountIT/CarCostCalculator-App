using CarCostCalculator_App.CCL.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing
{
    /// <summary>
    /// Resolves the corresponding HTTP request for a specific request type.
    /// </summary>
    /// <typeparam name="TRequest">The type of request being resolved.</typeparam>
    public class WebRequestRouter<TRequest>
        where TRequest : IWebRequest
    {
        #region Private Members

        private const string MULTIPART_FORM_DATA_FILE = "file";
        private readonly string _endpointRoute;
        private readonly HttpMethod _httpMethod;
        private readonly bool _isFileCommand;
        private readonly TypeInfo _typeInfo;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestRouter{TRequest}"/> class.
        /// </summary>
        public WebRequestRouter()
        {
            _typeInfo = typeof(TRequest).GetTypeInfo();
            _isFileCommand = typeof(IFileCommandBase).IsAssignableFrom(_typeInfo);
            _endpointRoute = CommandQueryEndpointResolver.ResolveEndpointRoute(_typeInfo);
            _httpMethod = CommandQueryEndpointResolver.ResolveHttpMethod(_typeInfo);
        }

        #endregion

        #region Private Properties

        private bool UseUriParams => !_httpMethod.UseRequestBody() || _isFileCommand;

        #endregion

        #region Public Methods

        /// <summary>
        /// Establish a <see cref="HttpRequestMessage"/> for the specified request.
        /// </summary>
        /// <param name="request">The request to establish a <see cref="HttpRequestMessage"/> for.</param>
        /// <returns><see cref="HttpRequestMessage"/></returns>
        public HttpRequestMessage EstablishRequestMessage(TRequest request)
        {
            var requestMessage = new HttpRequestMessage(_httpMethod, GetRequestUri(request));

            if (_httpMethod.UseRequestBody())
            {
                if (_isFileCommand)
                {
                    var fileCommand = (IFileCommandBase)request;
                    var fileItem = fileCommand.FileItem;
                    var fileContent = new StreamContent(fileItem.OpenReadStream());

                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileItem.ContentType);
                    requestMessage.Content = new MultipartFormDataContent
                {
                    {
                        fileContent,
                        MULTIPART_FORM_DATA_FILE,
                        fileItem.FileName
                    }
                };
                }
                else
                {
                    requestMessage.Content = JsonContent.Create(request);
                }
            }

            return requestMessage;
        }

        #endregion

        #region Private Methods

        private string GetRequestUri(TRequest request)
            => UseUriParams ? WebRequestUriBuilder<TRequest>.BuildParameterizedRequestUri(request, _typeInfo, _endpointRoute)
                            : _endpointRoute;

        #endregion
    }
}
