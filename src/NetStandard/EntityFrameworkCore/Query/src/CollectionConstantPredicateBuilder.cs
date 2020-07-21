using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public static class CollectionConstantPredicateBuilder<SourceType>
    {
        /// <summary>
        /// Creates a deferred collection constant predicate builder from a non-null and non-empty collection.
        /// </summary>
        /// <typeparam name="ComparisonType">The type of an item of <paramref name="comparisonEnumerable"/>.</typeparam>
        /// <param name="comparisonEnumerable">A collection of comparison values with at least one item.</param>
        /// <returns>A deferred collection constant predicate builder.</returns>
        public static DeferredCreateBuilder<ComparisonType> CreateFromCollection<ComparisonType>(
            IEnumerable<ComparisonType> comparisonEnumerable) =>
            new DeferredCreateBuilder<ComparisonType>(comparisonEnumerable);

        public readonly struct DeferredCreateBuilder<ComparisonType>
        {
            private readonly IEnumerable<ComparisonType> enumerable;

            public DeferredCreateBuilder(IEnumerable<ComparisonType> enumerable) =>
                this.enumerable = enumerable;

            /// <summary>
            /// Defines a predicate per item. The predicates are combined 
            /// via <paramref name="consecutiveItemBinaryExpressionFactory"/>.
            /// </summary>
            /// <param name="consecutiveItemBinaryExpressionFactory">The binary expression factory combines an item predicate with a previous item predicate.</param>
            /// <param name="sourceAndItemPredicate">The expression that represents the predicate.</param>
            /// <returns>The origin builder from which you called this method.</returns>
            /// <exception cref="ArgumentNullException">A deferred parameter is null.</exception>
            /// <exception cref="ArgumentException">The deferred comparison list is empty.</exception>
            public CollectionConstantPredicateBuilder<SourceType, ComparisonType> DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate) =>
                new CollectionConstantPredicateBuilder<SourceType, ComparisonType>(consecutiveItemBinaryExpressionFactory,
                    enumerable, sourceAndItemPredicate);
        }
    }
}
