namespace CarCostCalculator_App.CCL.CQRS.Context
{
    /// <summary>
    /// The default provider creates a new unique guid for every <see cref="RequestContext"/>.
    /// </summary>
    public class RequestContextIdProvider : IRequestContextIdProvider
    {
        #region Private Members

        private Guid? _id;

        #endregion

        #region Public Properties

        /// <summary>
        /// Property which represents a unique ID for a <see cref="RequestContext"/>.
        /// The getter sets the ID, if not already set, and returns it.
        /// The setter does not override an existing value.
        /// </summary>
        public Guid Id
        {
            get
            {
                _id ??= Guid.NewGuid();
                return _id.Value;
            }
            set
            {
                if (_id is not null)
                {
                    throw new InvalidOperationException("Context ID has already been initialized.");
                }

                _id = value;
            }
        }

        #endregion
    }
}
