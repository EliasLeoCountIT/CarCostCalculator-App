using CarCostCalculator_App.CCL.Common.Faults;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup;
using System.Reflection;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing
{
    /// <summary>
    /// Locates the file link for a specified file query.
    /// </summary>
    /// <typeparam name="TFileQuery">The type of the <see cref="IFileQuery"/>.</typeparam>
    public class FileQueryLocator<TFileQuery>
        where TFileQuery : IFileQuery
    {
        #region Private Members

        private readonly HttpClientProvider<TFileQuery> _clientProvider;
        private readonly string _endpointRoute;
        private readonly TypeInfo _typeInfo;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileQueryLocator{TFileQuery}"/> class.
        /// </summary>
        /// <param name="clientProvider"><see cref="HttpClientProvider{TFileQuery}"/></param>
        public FileQueryLocator(HttpClientProvider<TFileQuery> clientProvider)
        {
            _clientProvider = clientProvider;
            _typeInfo = typeof(TFileQuery).GetTypeInfo();
            _endpointRoute = CommandQueryEndpointResolver.ResolveEndpointRoute(_typeInfo);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieve the file link for the specified file query.
        /// </summary>
        /// <param name="fileQuery"><see cref="IFileQuery"/></param>
        /// <returns><see cref="Uri"/></returns>
        public Uri RetrieveFileUri(TFileQuery fileQuery)
        {
            using var httpClient = _clientProvider.CreateClient();
            var fileQueryUri = WebRequestUriBuilder<TFileQuery>.BuildParameterizedRequestUri(fileQuery, _typeInfo, _endpointRoute);

            return new Uri(httpClient.BaseAddress ?? throw new MissingConfigurationException("No base address configured."), fileQueryUri);
        }

        #endregion
    }
}
