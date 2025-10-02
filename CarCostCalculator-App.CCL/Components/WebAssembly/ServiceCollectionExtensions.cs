using CarCostCalculator_App.CCL.Components.WebAssembly.BrowserInterop;
using CarCostCalculator_App.CCL.Components.WebAssembly.CQRS;
using CarCostCalculator_App.CCL.Components.WebAssembly.Faults;
using CarCostCalculator_App.CCL.Components.WebAssembly.Helper;
using CarCostCalculator_App.CCL.Components.WebAssembly.Localization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using MudBlazor.Translations;

namespace CarCostCalculator_App.CCL.Components.WebAssembly
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds the services required for authorization and authentication.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAuthorizationAndAuthentication(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();

            return services;
        }

        /// <summary>
        /// Adds <see cref="ErrorStateProvider"/> to provide error handling.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddErrorStateProvider(this IServiceCollection services)
        {
            services.AddSingleton<ErrorStateProvider>();

            return services;
        }

        /// <summary>
        /// Adds a behavior that catches <see cref="RemoteProblemException"/> and informs the user.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddRemoteProblemExceptionBehavior(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RemoteProblemExceptionBehavior<,>));

            return services;
        }

        /// <summary>
        /// Configures host environment identification.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddWebAssemblyHostEnvironmentIdentification(this IServiceCollection services)
        {
            services.AddSingleton<WebAssemblyHostEnvironmentHelper>();

            return services;
        }

        /// <summary>
        /// Configures the services for the file download web module.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureFileDownloadModule(this IServiceCollection services)
        {
            services.AddScoped<FileDownloadModule>();

            return services;
        }

        /// <summary>
        /// Configures the localization services.
        /// </summary>
        /// <param name="services">Dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureLocalization<TAppTranslations>(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddMudTranslations();
            services.AddSingleton<IAppLocalizer, MultiResXLocalizer<TAppTranslations>>();
            services.AddLocalizationEnumInterceptor<AppLocalizationEnumInterceptor>();

            return services;
        }

        #endregion
    }
}
