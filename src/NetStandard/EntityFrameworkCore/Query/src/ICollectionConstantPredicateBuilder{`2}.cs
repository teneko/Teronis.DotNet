// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public interface IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonItemType>
    {
        /// <summary>
        /// Creates a deferred collection constant predicate builder from <paramref name="comparisonItemsFactory"/> that might return a null or empty collection.
        /// </summary>
        /// <typeparam name="ThenComparisonItemType">The type of an item of the return value of <paramref name="comparisonItemsFactory"/>.</typeparam>
        /// <param name="parentBinaryExpressionFactory">The binary expression factory combines a collection predicate with a previous collection predicate.</param>
        /// <param name="comparisonItemsFactory">A collection expression that might return a comparison value list with at least one item.</param>
        /// <returns>A deferred collection constant predicate builder.</returns>
        CollectionConstantPredicateBuilder<SourceType, ComparisonItemType>.IDeferredThenCreateCollectionConstantBuilder<ThenComparisonItemType> ThenCreateFromCollection<ThenComparisonItemType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonItemType, IEnumerable<ThenComparisonItemType>?> comparisonItemsFactory,
            ComparisonItemsBehaviourFlags comparisonItemsBehaviourFlags);
    }
}
