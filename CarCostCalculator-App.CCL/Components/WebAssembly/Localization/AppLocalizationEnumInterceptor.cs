using MudBlazor;

namespace CarCostCalculator_App.CCL.Components.WebAssembly.Localization
{
    /// <summary>
    /// Custom Enum Localization
    /// </summary>
    public class AppLocalizationEnumInterceptor(IAppLocalizer localizer) : ILocalizationEnumInterceptor
    {
        #region Private Members

        private readonly IAppLocalizer _localizer = localizer;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the translation for the intercepted enum
        /// </summary>
        /// <param name="enumeration">The <see cref="Enum"/> to be translated</param>
        /// <returns>A translated string</returns>
        public string Handle(Enum enumeration) => _localizer[enumeration];

        #endregion
    }
}
