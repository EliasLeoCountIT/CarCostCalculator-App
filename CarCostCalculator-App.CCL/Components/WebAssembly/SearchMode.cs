namespace CarCostCalculator_App.CCL.Components.WebAssembly
{
    /// <summary>
    /// Represents the search mode.
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// Represents the single search mode. Only one term can be entered and search starts immediately.
        /// </summary>
        Single,

        /// <summary>
        /// Represents the multi search mode. Multiple terms can be entered and submitted using the enter key. Search terms are displayed as chips.
        /// </summary>
        Multi
    }
}
