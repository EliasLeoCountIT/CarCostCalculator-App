namespace CarCostCalculator_App.CCL.CQRS.Context
{
    /// <summary>
    /// Provider of a unique ID for the <see cref="RequestContext"/>.
    /// </summary>
    public interface IRequestContextIdProvider
    {
        #region Public Properties

        /// <summary>
        /// Property which represents a unique id for a <see cref="RequestContext"/>.
        /// </summary>
        Guid Id { get; set; }

        #endregion
    }
}
