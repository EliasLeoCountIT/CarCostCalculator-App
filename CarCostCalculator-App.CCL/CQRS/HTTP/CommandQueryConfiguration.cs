using CarCostCalculator_App.CCL.Contracts;
using System.Reflection;

namespace CarCostCalculator_App.CCL.CQRS.HTTP
{
    /// <summary>
    /// Provides programmatic configuration used by Command/Query endpoints.
    /// </summary>
    public class CommandQueryConfiguration
    {
        #region Private Members

        private Assembly? _contractsAssembly;

        #endregion

        #region Public Properties

        /// <summary>
        /// The assembly containing the contracts.
        /// </summary>
        public Assembly ContractsAssembly => _contractsAssembly ?? throw new InvalidOperationException("Please register a contracts assembly.");

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all request types from the contracts assembly.
        /// </summary>
        /// <returns><see cref="IEnumerable{Type}"/></returns>
        public IEnumerable<Type> GetRequestTypes() => ContractsAssembly.GetTypes().Where(typeof(IWebRequest).IsAssignableFrom);

        /// <summary>
        /// Register various endpoints from assembly.
        /// </summary>
        /// <param name="assembly">Assembly to scan.</param>
        /// <returns>This instance.</returns>
        public CommandQueryConfiguration RegisterContractsFromAssembly(Assembly assembly)
        {
            _contractsAssembly = assembly;

            return this;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// <see cref="CommandQueryConfiguration"/> factory method.
        /// </summary>
        /// <param name="configure">An <see cref="Action{CommandQueryEndpointsConfiguration}"/> to configure the provided <see cref="CommandQueryConfiguration"/>.</param>
        /// <returns><see cref="CommandQueryConfiguration"/></returns>
        internal static CommandQueryConfiguration SettleUp(Action<CommandQueryConfiguration> configure)
        {
            var config = new CommandQueryConfiguration();

            configure.Invoke(config);

            return config;
        }

        #endregion
    }
}
