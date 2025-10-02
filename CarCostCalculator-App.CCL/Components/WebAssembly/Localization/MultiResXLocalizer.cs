using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CarCostCalculator_App.CCL.Components.WebAssembly.Localization
{
    /// <summary>
    /// Generic Localizer that uses two resx files for localization.
    /// </summary>
    public class MultiResXLocalizer<T>(IStringLocalizerFactory factory) : IAppLocalizer
    {
        #region Private Members

        private readonly IStringLocalizer _fallbackLocalizer = factory.Create(typeof(CommonTranslations));
        private readonly IStringLocalizer _primaryLocalizer = factory.Create(typeof(T));

        #endregion

        #region Public Indexers

        /// <inheritdoc />
        public string this[string key, params object[] args]
        {
            get
            {
                var value = _primaryLocalizer[key, args];
                if (!string.IsNullOrEmpty(value) && !value.ResourceNotFound)
                {
                    return value;
                }

                return _fallbackLocalizer[key, args]; // Fallback if not found
            }
        }

        /// <inheritdoc />
        public string this[Enum? enumeration]
        {
            get
            {
                if (enumeration is null)
                {
                    return string.Empty;
                }

                var typeInfo = enumeration.GetType();
                var memberInfo = typeInfo.GetField(enumeration.ToString(), BindingFlags.Public | BindingFlags.Static);
                var display = memberInfo?.GetCustomAttribute<DisplayAttribute>(inherit: false);
                var displayName = display?.GetName();

                if (displayName is not null)
                {
                    return this[displayName];
                }

                return this[typeInfo.Name + "_" + enumeration.ToString()];
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public bool KeyExists(string key)
        {
            var keyExists = !string.IsNullOrEmpty(_primaryLocalizer[key]) && !_primaryLocalizer[key].ResourceNotFound;

            if (!keyExists)
            {
                // Fallback if not found
                keyExists = !string.IsNullOrEmpty(_fallbackLocalizer[key]) && !_fallbackLocalizer[key].ResourceNotFound;
            }

            return keyExists;
        }

        #endregion
    }
}
