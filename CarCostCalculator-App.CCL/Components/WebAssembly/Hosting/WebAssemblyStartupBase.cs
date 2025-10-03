using CarCostCalculator_App.CCL.Common.Faults;
using CarCostCalculator_App.CCL.Components.WebAssembly.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.Components.WebAssembly.Hosting
{
    /// <summary>
    /// Web assembly startup configuration.
    /// </summary>
    public abstract class WebAssemblyStartupBase
    {
        #region Private Members

        private IConfiguration? _configuration;
        private IWebAssemblyHostEnvironment? _environment;

        #endregion

        #region Public Properties

        /// <summary>
        /// Provides application configuration properties.
        /// </summary>
        public IConfiguration Configuration
        {
            get => _configuration ?? throw new MissingConfigurationException("Configuration is not initialized.");
            set => _configuration = value;
        }

        /// <summary>
        /// Provides information about the hosting environment an application is running in.
        /// </summary>
        public IWebAssemblyHostEnvironment Environment
        {
            get => _environment ?? throw new MissingConfigurationException("Environment is not initialized.");
            set => _environment = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Use this method to configure root components.
        /// </summary>
        /// <param name="components">Root components definition.</param>
        public abstract void ConfigureRootComponents(RootComponentMappingCollection components);

        /// <summary>
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        public abstract void ConfigureServices(IServiceCollection services);

        #endregion

        #region Protected Methods

        /// <summary>
        /// Get the base address of a connected service.
        /// </summary>
        /// <param name="configurationKey">Configuration key of the connected service.</param>
        /// <returns><see cref="Uri"/></returns>
        /// <exception cref="MissingConfigurationException"><paramref name="configurationKey"/> is not configured.</exception>
        protected Uri GetBackendBaseUri(string configurationKey)
        {
            var settings = new BackendConnectionConfiguration();

            Configuration.Bind(configurationKey, settings);

            if (!string.IsNullOrEmpty(settings.BaseAddress))
            {
                return new Uri(settings.BaseAddress);
            }
            else if (!string.IsNullOrEmpty(settings.ReplacementAddressPart) && !string.IsNullOrEmpty(settings.ReplacingAddressPart))
            {
                return new Uri(Environment.BaseAddress.Replace(settings.ReplacingAddressPart, settings.ReplacementAddressPart));
            }
            else
            {
                throw new MissingConfigurationException($"{configurationKey} is not configured.");
            }
        }

        #endregion
    }
}
