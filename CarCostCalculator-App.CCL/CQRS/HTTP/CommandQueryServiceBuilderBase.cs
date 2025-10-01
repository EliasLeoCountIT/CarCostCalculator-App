using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.CQRS.HTTP
{
    /// <summary>
    /// Used to configure Command/Query endpoints.
    /// </summary>
    /// <param name="services">The services being configured.</param>
    /// <param name="configure">An <see cref="Action{CommandQueryEndpointsConfiguration}"/> to configure the provided <see cref="CommandQueryConfiguration"/>.</param>
    public abstract class CommandQueryServiceBuilderBase(IServiceCollection services, Action<CommandQueryConfiguration> configure)
    {
        #region Public Properties

        /// <summary>
        /// The configuration of the Command/Query endpoints.
        /// </summary>
        public CommandQueryConfiguration Configuration { get; } = CommandQueryConfiguration.SettleUp(configure);

        /// <summary>
        /// The services being configured.
        /// </summary>
        public IServiceCollection Services { get; } = services;

        #endregion
    }
}
