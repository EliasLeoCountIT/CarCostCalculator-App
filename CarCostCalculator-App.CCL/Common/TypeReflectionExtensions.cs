using System.Collections;
using System.Reflection;

namespace CarCostCalculator_App.CCL.Common
{
    /// <summary>
    /// Provides <see cref="System.Reflection"/> related extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeReflectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Check if the current <see cref="Type"/> is an enumerable type.
        /// </summary>
        /// <param name="type">The current <see cref="Type"/>.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsEnumerableType(this Type type)
            => type.GetInterface(nameof(IEnumerable)) is not null;

        /// <summary>
        /// Check if the current <see cref="Type"/> implements a specific generic interface.
        /// </summary>
        /// <param name="requestType">The current <see cref="Type"/>.</param>
        /// <param name="genericType">The generic type to check.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsSpecificGenericType(this Type requestType, Type genericType)
            => Array.Exists(requestType.GetInterfaces(), x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);

        /// <summary>
        /// Get all public constants of the current <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The current <see cref="Type"/>.</param>
        /// <returns><see cref="IEnumerable{String}"/></returns>
        public static IEnumerable<string> RetrieveConstants(this Type type)
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static).Where(fi => fi.IsLiteral))
            {
                var value = field.GetValue(null)?.ToString();

                if (value is not null)
                {
                    yield return value;
                }
            }
        }

        #endregion
    }
}
