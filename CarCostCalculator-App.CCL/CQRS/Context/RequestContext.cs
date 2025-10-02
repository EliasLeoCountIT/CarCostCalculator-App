namespace CarCostCalculator_App.CCL.CQRS.Context
{
    /// <inheritdoc />
    public class RequestContext : IRequestContext
    {
        #region Private Members

        private readonly Stack<Type> _requestTypeStack;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Instantiation of <see cref="RequestContext"/>.
        /// </summary>
        /// <param name="requestContextIdProvider">Provider of a unique id.</param>
        public RequestContext(IRequestContextIdProvider requestContextIdProvider)
        {
            ArgumentNullException.ThrowIfNull(requestContextIdProvider);

            Id = requestContextIdProvider.Id;
            _requestTypeStack = new Stack<Type>();
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public IDictionary<string, object> AdditionalData { get; } = new Dictionary<string, object>();

        /// <inheritdoc />
        public Type? CurrentRequestType => RequestTypeStack.FirstOrDefault();

        /// <summary>
        /// The ID of the current context.
        /// </summary>
        public Guid Id { get; }

        /// <inheritdoc />
        public bool IsNestedType => _requestTypeStack.Count > 1;

        /// <inheritdoc />
        public IEnumerable<Type> RequestTypeStack => _requestTypeStack.AsEnumerable();

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes the object at the top of the request stack.
        /// </summary>
        public void Pop() => _requestTypeStack.Pop();

        /// <summary>
        /// Inserts an object to the top of the request stack.
        /// </summary>
        /// <param name="request">The current request.</param>
        public void Push(object request) => _requestTypeStack.Push(request.GetType());

        #endregion
    }
}
