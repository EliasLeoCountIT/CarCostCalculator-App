using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client
{
    /// <summary>
    /// Resolves the name of the <see cref="HttpClient"/> for a specific request type.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
    public class HttpClientNameResolver(IServiceProvider serviceProvider)
    {
        #region Private Members

        private readonly IServiceProvider _serviceProvider = serviceProvider;

        #endregion

        #region Internal Records

        internal record ConfigEntry(string CustomName);

        #endregion

        #region Public Methods

        /// <summary>
        /// Resolves the name of the <see cref="HttpClient"/> for a specific request type.
        /// </summary>
        /// <typeparam name="TRequest">The type of request being resolved.</typeparam>
        /// <returns>The name of the <see cref="HttpClient"/> for a specific request type.</returns>
        public string ResolveName<TRequest>()
        {
            var defaultName = ResolveDefaultName<TRequest>();
            var configEntry = _serviceProvider.GetKeyedService<ConfigEntry>(defaultName);

            return configEntry?.CustomName ?? defaultName;
        }

        #endregion

        #region Internal Methods

        internal static string ResolveDefaultName<TRequest>() => ResolveDefaultName(typeof(TRequest).Assembly);

        internal static string ResolveDefaultName(Assembly assembly) => assembly.GetName().Name!;

        #endregion
    }
}
