namespace CarCostCalculator_App.CCL.CQRS.HTTP
{
    /// <summary>
    /// Provides extension methods for <see cref="HttpMethod"/>.
    /// </summary>
    public static class HttpMethodExtensions
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the HTTP method uses the request body.
        /// </summary>
        /// <param name="method"><see cref="HttpMethod"/></param>
        /// <returns>True, if a request body is recommended.</returns>
        public static bool UseRequestBody(this HttpMethod method)
            => method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch;

        #endregion
    }
}
