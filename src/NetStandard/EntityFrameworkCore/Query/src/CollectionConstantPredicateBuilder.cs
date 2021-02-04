using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public static class CollectionConstantPredicateBuilder<SourceType>
    {
        /// <summary>
        /// Creates a collection constant predicate builder from a non-null and non-empty collection. Otherwise
        /// <see cref="ArgumentNullException"/> will be thrown. When starting with a non-null and non-empty colection
        /// you are able to use <see cref="CollectionConstantPredicateBuilder{SourceType, ComparisonItemType}.ThenCreateFromCollection{ThenComparisonItemType}(Func{Expression, Expression, BinaryExpression}, Func{ComparisonItemType, IEnumerable{ThenComparisonItemType}?}, ComparisonItemsBehaviourFlags)"/>
        /// and this collection can then be null or empy.
        /// </summary>
        /// <typeparam name="ComparisonItemType">The type of an item of <paramref name="comparisonItems"/>.</typeparam>
        /// <param name="comparisonItems">A collection of comparison items with at least one item.</param>
        /// <returns>A deferred collection constant predicate builder.</returns>
        public static DeferredCreateBuilder<ComparisonItemType> CreateFromCollection<ComparisonItemType>(
            IEnumerable<ComparisonItemType> comparisonItems) =>
            new DeferredCreateBuilder<ComparisonItemType>(comparisonItems);

        public readonly struct DeferredCreateBuilder<ComparisonItemType>
        {
            private readonly IEnumerable<ComparisonItemType> comparisonItems;

            public DeferredCreateBuilder(IEnumerable<ComparisonItemType> comparisonItems) =>
                this.comparisonItems = comparisonItems;

            /// <summary>
            /// Defines a predicate per item. The predicates are combined 
            /// via <paramref name="consecutiveItemBinaryExpressionFactory"/>.
            /// </summary>
            /// <param name="consecutiveItemBinaryExpressionFactory">
            /// The binary expression factory that combines an item predicate, an item predicate that is arisen from 
            /// <see cref="DeferredCreateBuilder{ComparisonItemType}.comparisonItems"/> and that encloses every item predicate of item
            /// predicates of child collection and their collections, with another item predicate arisen from
            /// <see cref="DeferredCreateBuilder{ComparisonItemType}.comparisonItems"/> that encloses every item predicate of item
            /// predicates of child collection and their collections.
            /// </param>
            /// <param name="sourceAndComparisonItemPredicate">The expression that represents the predicate.</param>
            /// <returns>The origin builder from which you called this method.</returns>
            /// <exception cref="ArgumentNullException">The private field <see cref="DeferredCreateBuilder{ComparisonItemType}.comparisonItems"/> is null.</exception>
            /// <exception cref="ArgumentException">The private field <see cref="DeferredCreateBuilder{ComparisonItemType}.comparisonItems"/> is empty.</exception>
            public CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonItemType>> sourceAndComparisonItemPredicate) =>
                new CollectionConstantPredicateBuilder<SourceType, ComparisonItemType>(
                    consecutiveItemBinaryExpressionFactory,
                    comparisonItems, 
                    sourceAndComparisonItemPredicate);
        }
    }
}
