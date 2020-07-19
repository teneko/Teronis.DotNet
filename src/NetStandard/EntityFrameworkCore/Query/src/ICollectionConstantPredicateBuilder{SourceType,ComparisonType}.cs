using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public interface IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType>
    {
        /// <summary>
        /// Creates a deferred collection constant predicate builder from <paramref name="getComparisonValues"/> that might return a null or empty collection.
        /// </summary>
        /// <typeparam name="ThenComparisonType">The type of an item of the return value of <paramref name="getComparisonValues"/>.</typeparam>
        /// <param name="parentBinaryExpressionFactory">The binary expression factory combines a collection predicate with a previous collection predicate.</param>
        /// <param name="getComparisonValues">A collection expression that might return a comparison value list with at least one item.</param>
        /// <returns>A deferred collection constant predicate builder.</returns>
        CollectionConstantPredicateBuilder<SourceType, ComparisonType>.IDeferredThenCreateCollectionConstantBuilder<ThenComparisonType> ThenCreateFromCollection<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues);
    }
}
