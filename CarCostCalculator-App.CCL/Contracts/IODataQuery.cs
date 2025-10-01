namespace CarCostCalculator_App.CCL.Contracts
{
    /// <summary>
    /// An open data protocol query that requests <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the requested items.</typeparam>
    public interface IODataQuery<out TItem> : IQuery<IQueryable<TItem>>
    {
        #region Public Properties

        /// <summary>
        /// The open data protocol filter string.
        /// </summary>
        string? Filter { get; set; }

        /// <summary>
        /// The open data protocol order by clause.
        /// </summary>
        /// <remarks>
        /// Suffix 'asc' or 'desc' deciding whether to order ascending or descending.
        /// </remarks>
        string? OrderBy { get; set; }

        /// <summary>
        /// The start index of the requested data.
        /// </summary>
        int Skip { get; set; }

        /// <summary>
        /// The size of the data batch per request.
        /// </summary>
        int Top { get; set; }

        #endregion
    }
}
