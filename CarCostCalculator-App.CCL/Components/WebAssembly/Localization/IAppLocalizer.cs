namespace CarCostCalculator_App.CCL.Components.WebAssembly.Localization
{
    /// <summary>
    /// Provides localized string resources for the application, supporting parameterized formatting and enumeration-based
    /// keys.
    /// </summary>
    /// <remarks>This interface allows retrieving localized strings using either a string key with optional
    /// formatting arguments or an enumeration value. It is designed to facilitate localization  in applications by
    /// abstracting the retrieval of culture-specific resources.</remarks>
    public interface IAppLocalizer
    {
        #region Public Indexers

        /// <summary>
        /// Gets the localized string associated with the specified key, formatted with the provided arguments.
        /// </summary>
        /// <param name="key">The key identifying the localized string to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="args">An array of objects to format the localized string with. Can be empty if no formatting is required.</param>
        /// <returns></returns>
        string this[string key, params object[] args] { get; }

        /// <summary>
        /// Returns the translation for the display name attribute or, if no display name is found the translation for the Enum "Typename_Membername"/>
        /// </summary>
        /// <param name="enumeration">The <see cref="Enum"/> to be translated</param>
        /// <returns>A translated string</returns>
        string this[Enum? enumeration] { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if a localized string exists for the specified key.
        /// </summary>
        /// <param name="key">The key to check for existence.</param>
        /// <returns><see langword="true"/> if the key exists; otherwise, <see langword="false"/>.</returns>
        bool KeyExists(string key);

        #endregion
    }
}
