using CarCostCalculator_App.CCL.Contracts;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Client.Routing
{
    internal static partial class WebRequestUriBuilder<TRequest>
    where TRequest : IWebRequest
    {
        #region Private Members

        private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";
        private const string INVARIANT_NUMBER_FORMAT = "{0:0.####}";

        #endregion

        #region Internal Methods

        internal static string BuildParameterizedRequestUri(TRequest request, TypeInfo typeInfo, string uri)
        {
            var queryParams = new Collection<string>();
            var routeParams = FindRouteParams(uri, out var routeParamsRegex);

            CollectUriParameters(request, typeInfo, queryParams, routeParams);

            //apply route parameters
            if (routeParams.Count != 0)
            {
                var missingRouteParams = routeParams.Any(rp => string.IsNullOrEmpty(rp.Value));

                uri = missingRouteParams
                    ? throw new InvalidOperationException("All route parameters must contain a value!")
                    : routeParamsRegex.Replace(uri, match => routeParams[match.Groups[1].Value]);
            }

            //apply query parameters
            if (queryParams.Count != 0)
            {
                var queryString = string.Join("&", queryParams);

                uri = $"{uri}?{queryString}";
            }

            return uri;
        }

        #endregion

        #region Private Methods

        private static void CollectUriParameters(object instance, TypeInfo typeInfo, ICollection<string> queryParams, IDictionary<string, string> routeParams)
        {
            var customUriParameterType = typeof(IUriParameter);
            var curProperties = typeInfo.DeclaredProperties
                                        .Where(prop => (prop.CanWrite || customUriParameterType.IsAssignableFrom(prop.PropertyType))
                                                    && !prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(CompilerGeneratedAttribute)));

            foreach (var prop in curProperties.Where(prop => prop.GetValue(instance) is not null))
            {
                if (customUriParameterType.IsAssignableFrom(prop.PropertyType))
                {
                    CollectUriParameters(prop.GetValue(instance)!, prop.PropertyType.GetTypeInfo(), queryParams, routeParams);
                }
                else if (routeParams.ContainsKey(prop.Name))
                {
                    routeParams[prop.Name] = FormatParameterValue(prop, instance);
                }
                else
                {
                    queryParams.Add(FormatQueryParameter(prop, instance));
                }
            }

            if (typeInfo.BaseType is not null)
            {
                CollectUriParameters(instance, typeInfo.BaseType.GetTypeInfo(), queryParams, routeParams);
            }
        }

        private static Dictionary<string, string> FindRouteParams(string uri, out Regex routeParamsRegex)
        {
            routeParamsRegex = MatchRouteParams();

            var matches = routeParamsRegex.Matches(uri);

            return matches.Select(matches => matches.Groups[1].Value)
                          .Distinct(StringComparer.OrdinalIgnoreCase)
                          .ToDictionary(routeParam => routeParam, routeParam => string.Empty, StringComparer.OrdinalIgnoreCase);
        }

        private static string FormatParameterValue(PropertyInfo prop, object instance)
        {
            var value = GetParameterValue(prop, instance);

            return FormatParameterValue(value);
        }

        private static string FormatParameterValue(object value)
        {
            if (value is DateTime || value is DateTime?)
            {
                return ((DateTime)value).ToString(DATE_TIME_FORMAT);
            }

            if (value is decimal or float or double)
            {
                return string.Format(CultureInfo.InvariantCulture, INVARIANT_NUMBER_FORMAT, value);
            }

            return WebUtility.UrlEncode(value.ToString())!;
        }

        private static string FormatQueryParameter(PropertyInfo prop, object instance)
        {
            var value = GetParameterValue(prop, instance);

            if (value is IEnumerable enumerable and not string)
            {
                var parameters = new List<string>();

                foreach (var item in enumerable)
                {
                    parameters.Add($"{prop.Name}={FormatParameterValue(item)}");
                }

                return string.Join("&", parameters);
            }

            return $"{prop.Name}={FormatParameterValue(value)}";
        }

        private static object GetParameterValue(PropertyInfo prop, object instance)
            => prop.GetValue(instance) ?? throw new InvalidOperationException("The query parameter should not be null here!");

        [GeneratedRegex("{(.*?)}")]
        private static partial Regex MatchRouteParams();

        #endregion
    }
}
