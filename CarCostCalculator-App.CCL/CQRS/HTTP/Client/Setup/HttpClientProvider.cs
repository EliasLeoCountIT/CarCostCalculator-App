using CarCostCalculator_App.CCL.Contracts;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Setup
{
    /// <summary>
    /// Provides an <see cref="HttpClient"/> for a specific request type.
    /// </summary>
    /// <typeparam name="TRequest">The type of request being resolved.</typeparam>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/></param>
    /// <param name="httpClientNameResolver"><see cref="HttpClientNameResolver"/></param>
    public class HttpClientProvider<TRequest>(IHttpClientFactory httpClientFactory, HttpClientNameResolver httpClientNameResolver)
        where TRequest : IWebRequest
    {
        #region Private Members

        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _httpClientName = httpClientNameResolver.ResolveName<TRequest>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates and configures an <see cref="HttpClient"/> for the request type.
        /// </summary>
        /// <returns><see cref="HttpClient"/></returns>
        public virtual HttpClient CreateClient() => _httpClientFactory.CreateClient(_httpClientName);

        #endregion
    }
}
