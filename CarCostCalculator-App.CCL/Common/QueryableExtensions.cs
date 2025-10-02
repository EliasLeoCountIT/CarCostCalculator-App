using System.Linq.Expressions;

namespace CarCostCalculator_App.CCL.Common
{
    /// <summary>
    /// Provides extension methods for <see cref="IQueryable"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        #region Public Methods

        /// <summary>
        /// Orders the result by the provided field <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <param name="query">The query to order.</param>
        /// <param name="name">The field name to use to order the query.</param>
        /// <param name="descending">If set to <c>true</c> the query result gets ordered descending; otherwise ascending.</param>
        /// <returns>The ordered <see cref="IQueryable"/>.</returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name, bool descending = true)
        {
            if (string.IsNullOrEmpty(name))
            {
                return query;
            }

            var lambda = CreateExpression<T>(name);

            return descending
                ? query.OrderByDescending(lambda)
                : query.OrderBy(lambda);
        }

        #endregion

        #region Private Methods

        private static Expression<Func<TSource, object>> CreateExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                var arr = member.ToCharArray();
                arr[0] = char.ToUpperInvariant(arr[0]);
                body = Expression.Property(body, new string(arr));
            }

            return Expression.Lambda<Func<TSource, object>>(Expression.Convert(body, typeof(object)), param);
        }

        #endregion
    }
}
