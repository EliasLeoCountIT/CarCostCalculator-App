namespace CarCostCalculator_App.CCL.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MetadataAttribute(params string[] tags) : Attribute
    {
        #region Public Properties

        /// <summary>
        /// A string representing a detailed description of the operation.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// A string representing a brief description of the operation.
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// Classification to categorize operations into related groups.
        /// </summary>
        public string[] Tags { get; set; } = tags;

        #endregion
    }
}