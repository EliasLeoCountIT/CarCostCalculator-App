using CarCostCalculator_App.CCL.Contracts;

namespace CarCostCalculator_App.CCL.CQRS.Abstractions.Queries
{
    /// <inheritdoc cref="IODataQuery{TItem}"/>
    public abstract record ODataBaseQuery<TItem> : IODataQuery<TItem>
    {
        /// <inheritdoc cref="IODataQuery{TItem}.Filter"/>
        public string? Filter { get; set; }

        /// <inheritdoc cref="IODataQuery{TItem}.OrderBy"/>
        public string? OrderBy { get; set; }

        /// <inheritdoc cref="IODataQuery{TItem}.Skip"/>
        public int Skip { get; set; }

        /// <inheritdoc cref="IODataQuery{TItem}.Top"/>
        public int Top { get; set; }
    }
}
