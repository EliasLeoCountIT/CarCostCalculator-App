namespace CarCostCalculator_App.CCL.Models
{
    /// <summary>
    /// A non-generic base record that contains metadata about the paged data result.
    /// </summary>
    /// <param name="TotalCount">The total count of available items.</param>
    /// <param name="PageSize">The size of the requested data batch.</param>
    /// <param name="StartIndex">The start index of the current data.</param>
    public abstract record PagedDataResult(long TotalCount, int? PageSize, int StartIndex = 0)
    { }

    /// <summary>
    /// A record that represents a paged data result.
    /// </summary>
    /// <typeparam name="TItem">The type of the paged items.</typeparam>
    /// <param name="Items">The paged items.</param>
    /// <param name="TotalCount">The total count of available items.</param>
    /// <param name="PageSize">The size of the requested data batch.</param>
    /// <param name="StartIndex">The start index of the current data.</param>
    public record PagedDataResult<TItem>(IEnumerable<TItem> Items, long TotalCount, int? PageSize = null, int StartIndex = 0)
        : PagedDataResult(TotalCount, PageSize, StartIndex)
    {
        /// <summary>
        /// Returns an empty result.
        /// </summary>
        public static readonly PagedDataResult<TItem> Empty = new([], 0);
    }
}
