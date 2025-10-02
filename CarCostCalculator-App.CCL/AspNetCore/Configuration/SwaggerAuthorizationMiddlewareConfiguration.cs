namespace CarCostCalculator_App.CCL.AspNetCore.Configuration
{
    /// <summary>
    /// Configuration options for the <see cref="Authorization.SwaggerAuthorizationMiddleware"/>.
    /// </summary>
    public class SwaggerAuthorizationMiddlewareConfiguration
    {
        #region Public Properties

        /// <summary>
        /// Required role for Swagger UI authorization.
        /// </summary>
        public required string RequiredRole { get; set; }

        #endregion
    }
}
