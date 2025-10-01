namespace CarCostCalculator_App.CCL.Metadata
{
    /// <summary>
    /// Defines an attribute to customize the route of a endpoint.
    /// </summary>
    /// <param name="route">A string representing the route of the endpoint.</param>
    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointAttribute(string route) : Attribute
    {
        #region Public Properties

        /// <summary>
        /// A string representing the route of the endpoint.
        /// </summary>
        public string Route { get; set; } = route;

        #endregion
    }
}
