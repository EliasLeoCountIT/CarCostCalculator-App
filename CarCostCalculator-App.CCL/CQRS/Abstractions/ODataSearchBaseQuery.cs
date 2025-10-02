using CarCostCalculator_App.CCL.CQRS.Abstractions.Queries;

namespace CarCostCalculator_App.CCL.CQRS.Abstractions
{
    /// <summary>
    /// Base query class for advanced search functionality.
    /// </summary>
    /// <typeparam name="TItem">The type of the item being queried.</typeparam>
    public abstract record ODataSearchBaseQuery<TItem> : ODataBaseQuery<TItem>
    {
        /// <summary>
        /// Search terms to filter results.
        /// </summary>
        public string[]? Terms { get; set; }
    }
}
