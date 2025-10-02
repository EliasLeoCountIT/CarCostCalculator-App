namespace CarCostCalculator_App.CCL.CQRS.Abstractions
{
    /// <summary>
    /// Defines an attribute to hold user roles for authorization.
    /// </summary>
    /// <param name="roles">Required roles to access the resource.</param>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAttribute(params string[] roles) : Attribute
    {
        #region Public Properties

        /// <summary>
        /// Required roles to access the resource.
        /// </summary>
        public string[] Roles { get; set; } = roles;

        #endregion
    }
}
