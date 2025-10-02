namespace CarCostCalculator_App.CCL.CQRS.Context
{
    /// <summary>
    /// A context for CQRS requests.
    /// </summary>
    public interface IRequestContext
    {
        #region Public Properties

        /// <summary>
        /// Key-value store to add additional data to the context.
        /// </summary>
        IDictionary<string, object> AdditionalData { get; }

        /// <summary>
        /// The type of the current request.
        /// </summary>
        Type? CurrentRequestType { get; }

        /// <summary>
        /// True, if in a nested call stack, otherwise false.
        /// </summary>
        bool IsNestedType { get; }

        /// <summary>
        /// The stack of requested types.
        /// </summary>
        IEnumerable<Type> RequestTypeStack { get; }

        #endregion
    }
}
