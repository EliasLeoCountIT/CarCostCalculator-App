using CarCostCalculator_App.CCL.Common;
using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.Enums;
using System.Linq.Expressions;

namespace CarCostCalculator_App.CCL.CQRS.Extensions
{
    /// <summary>
    /// Utility methods to support paging queries.
    /// </summary>
    public static class QueryableExtensions
    {
        #region Public Methods

        /// <summary>
        /// Applies the specified paging query on the <see cref="Queryable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <param name="query">The query to page.</param>
        /// <param name="paging">The paging query.</param>
        /// <returns>The ordered and paged <see cref="Queryable"/>.</returns>
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IPagingQuery paging)
        {
            var result = query;

            if (!string.IsNullOrEmpty(paging.OrderBy))
            {
                result = result.OrderBy(paging.OrderBy, paging.OrderDescending);
            }

            if (paging.StartIndex > 0)
            {
                result = result.Skip(paging.StartIndex);
            }

            if (paging.PageSize > 0)
            {
                result = result.Take(paging.PageSize);
            }

            return result;
        }

        /// <summary>
        /// Applies the search terms to the query.
        /// </summary>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <param name="source">The query to apply the search terms to.</param>
        /// <param name="searchTerms">The search terms to apply to the query.</param>
        /// <param name="properties">The properties to search within.</param>
        /// <param name="collectionProperties">The properties that are nested in collections.</param>
        /// <param name="searchMode">The mode of search to be performed.</param>
        /// <returns>The filtered <see cref="IQueryable"/>.</returns>
        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> source,
                                                   IEnumerable<string> searchTerms,
                                                   IEnumerable<Expression<Func<T, string?>>> properties,
                                                   IEnumerable<Expression<Func<T, IEnumerable<string?>>>>? collectionProperties = null,
                                                   SearchMode searchMode = SearchMode.Contains)
        {
            Func<Expression, Expression, Type[]?, Expression> comparisonExpression = searchMode switch
            {
                SearchMode.Contains => GenerateContainsExpression,
                SearchMode.StartsWith => GenerateStartsWithExpression,
                _ => throw new ArgumentOutOfRangeException(nameof(searchMode), $"Unsupported search mode: {searchMode}"),
            };

            return ApplySearch(source, searchTerms, properties, collectionProperties, comparisonExpression);
        }

        #endregion

        #region Private Methods

        private static IQueryable<T> ApplySearch<T>(this IQueryable<T> source,
                                                   IEnumerable<string> searchTerms,
                                                   IEnumerable<Expression<Func<T, string?>>> properties,
                                                   IEnumerable<Expression<Func<T, IEnumerable<string?>>>>? collectionProperties,
                                                   Func<Expression, Expression, Type[]?, Expression> comparisonFactory)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            // Create predicates for direct properties
            var propertyPredicates = searchTerms
                .Select(searchTerm =>
                    Expression.OrElse( // link properties and collection properties with OR
                        properties.Select(property =>
                        {
                            var propertyExpr = Expression.Invoke(property, parameter);
                            var searchTermExpr = Expression.Constant(searchTerm);

                            return GenerateExpression(propertyExpr, searchTermExpr, comparisonFactory);
                        })
                            .Aggregate(Expression.OrElse), // Combine properties with OR
                        collectionProperties == null
                            ? Expression.Constant(false)
                            : collectionProperties.Select(property =>
                            {
                                var propertyExpr = Expression.Invoke(property, parameter);
                                var itemParameter = Expression.Parameter(typeof(string), "item");
                                var searchTermExpr = Expression.Constant(searchTerm);

                                // Check if item in the collection is not null and includes the search term
                                var itemNotNull = Expression.NotEqual(itemParameter, Expression.Constant(null, typeof(string)));
                                var itemComparison = comparisonFactory(itemParameter, searchTermExpr, Type.EmptyTypes);
                                var itemPredicate = Expression.AndAlso(itemNotNull, itemComparison);

                                // Build the `Any` expression for the collection
                                var anyMethod = typeof(Enumerable)
                                .GetMethods()
                                .First(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 2)
                                .MakeGenericMethod(typeof(string));

                                var anyExpression = Expression.Call(anyMethod, propertyExpr, Expression.Lambda(itemPredicate, itemParameter));

                                return (Expression)anyExpression;
                            })
                                .Aggregate(Expression.OrElse) // Combine properties with OR
                    )
                )
                .Aggregate(Expression.AndAlso); // Combine search terms with AND

            return source.Where(Expression.Lambda<Func<T, bool>>(propertyPredicates, parameter));
        }

        private static MethodCallExpression GenerateContainsExpression(Expression propertyExpr, Expression searchTermExpr, Type[]? typeArguments)
            => Expression.Call(propertyExpr, nameof(string.Contains), typeArguments, searchTermExpr);

        private static BinaryExpression GenerateExpression(InvocationExpression propertyExpr, ConstantExpression termExpr, Func<Expression, Expression, Type[]?, Expression> comparisonFactory)
        {
            // check for null and comparison
            var notNull = Expression.NotEqual(propertyExpr, Expression.Constant(null, typeof(string)));
            var itemComparison = comparisonFactory(propertyExpr, termExpr, null);
            return Expression.AndAlso(notNull, itemComparison);
        }

        private static MethodCallExpression GenerateStartsWithExpression(Expression propertyExpr, Expression searchTermExpr, Type[]? typeArguments)
            => Expression.Call(propertyExpr, nameof(string.StartsWith), typeArguments, searchTermExpr);

        #endregion
    }
}
