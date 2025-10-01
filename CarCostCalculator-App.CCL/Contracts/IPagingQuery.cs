using CarCostCalculator_App.CCL.Models;

namespace CarCostCalculator_App.CCL.Contracts
{
    /// <summary>
    /// A query that requests paged data.
    /// </summary>
    public interface IPagingQuery
    {
        #region Public Properties

        /// <summary>
        /// The property to order the result by.
        /// </summary>
        string? OrderBy { get; set; }

        /// <summary>
        /// Property deciding whether to order descending or ascending.
        /// </summary>
        bool OrderDescending { get; set; }

        /// <summary>
        /// The size of the data batch per request.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// The start index of the requested page.
        /// </summary>
        int StartIndex { get; set; }

        #endregion
    }

    /// <summary>
    /// A query that requests paged <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the paged items.</typeparam>
    public interface IPagingQuery<TItem> : IPagingQuery, IQuery<PagedDataResult<TItem>>
    { }
}
