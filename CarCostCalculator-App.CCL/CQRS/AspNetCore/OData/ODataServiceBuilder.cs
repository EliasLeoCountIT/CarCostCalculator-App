using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.OData
{
    /// <summary>
    /// Used to configure OData endpoints.
    /// </summary>
    public class ODataServiceBuilder
    {
        #region Internal Constructors

        /// <summary>
        /// Instantiation of <see cref="ODataServiceBuilder"/>.
        /// </summary>
        /// <param name="services">The services being configured.</param>
        /// <param name="entityTypes">Explicit registered entity types of the OData model.</param>
        internal ODataServiceBuilder(IServiceCollection services, IEnumerable<Type> entityTypes)
        {
            Services = services;
            RegisteredTypes = entityTypes;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Explicit registered entity types of the OData model.
        /// </summary>
        public IEnumerable<Type> RegisteredTypes { get; }

        /// <summary>
        /// The services being configured.
        /// </summary>
        public IServiceCollection Services { get; }

        #endregion
    }
}
