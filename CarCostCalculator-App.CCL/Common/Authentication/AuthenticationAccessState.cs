namespace CarCostCalculator_App.CCL.Common.Authentication
{
    /// <summary>
    /// State of a <see cref="IAuthenticationAccessor" />.
    /// </summary>
    public enum AuthenticationAccessState : byte
    {
        /// <summary>
        /// <see cref="IAuthenticationAccessor" /> is active in the current context.
        /// </summary>
        Active = 0,

        /// <summary>
        /// <see cref="IAuthenticationAccessor" /> uses a derivation fallback mechanism.
        /// </summary>
        Fallback = 127,

        /// <summary>
        /// <see cref="IAuthenticationAccessor" /> is inactive in the current context.
        /// </summary>
        Inactive = 255
    }
}
