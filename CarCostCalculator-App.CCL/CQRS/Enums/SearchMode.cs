namespace CarCostCalculator_App.CCL.CQRS.Enums
{
    /// <summary>
    /// Specifies the mode of search to be performed in a query.
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// Determines whether the specified substring occurs within this string.
        /// </summary>
        Contains,

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string.
        /// </summary>
        StartsWith
    }
}
