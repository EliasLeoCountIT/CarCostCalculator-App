using CarCostCalculator_App.CCL.CQRS.HTTP.Client;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.CQRS.HTTP
{
    /// <inheritdoc />
    public class CommandQueryServiceBuilder : CommandQueryServiceBuilderBase
    {
        #region Internal Constructors

        /// <inheritdoc />
        internal CommandQueryServiceBuilder(IServiceCollection services, Action<CommandQueryConfiguration> configure)
            : base(services, configure) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the <see cref="FileQueryLocator{TFileQuery}"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <returns><see cref="CommandQueryServiceBuilder"/></returns>
        public CommandQueryServiceBuilder AddFileQueryLocator()
        {
            Services.AddSingleton(typeof(FileQueryLocator<>));

            return this;
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
        /// a <see cref="HttpClient"/> for the active CQRS (Command Query Responsibility Segregation) client.
        /// </summary>
        /// <param name="configureClient">A delegate that is used to configure an <see cref="HttpClient"/>.</param>
        /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
        public IHttpClientBuilder AddHttpClient(Action<HttpClient> configureClient)
            => Services.AddHttpClient(HttpClientNameResolver.ResolveDefaultName(Configuration.ContractsAssembly), configureClient);

        /// <summary>
        /// Uses the specified <see cref="HttpClient"/> for the active CQRS (Command Query Responsibility Segregation) client.
        /// </summary>
        /// <param name="clientName">The logical name of the <see cref="HttpClient"/> to use.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public IServiceCollection UseHttpClient(string clientName)
            => Services.AddKeyedSingleton(
                HttpClientNameResolver.ResolveDefaultName(Configuration.ContractsAssembly),
                new HttpClientNameResolver.ConfigEntry(clientName));

        #endregion
    }
}
