using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public static class CollectionConstantPredicateBuilder<SourceType>
    {
        public static DeferredCreateBuilder<ComparisonType> CreateFromCollection<ComparisonType>(
            IReadOnlyCollection<ComparisonType> comparisonList) =>
            new DeferredCreateBuilder<ComparisonType>(comparisonList);

        public readonly struct DeferredCreateBuilder<ComparisonType>
        {
            private readonly IReadOnlyCollection<ComparisonType> collection;

            public DeferredCreateBuilder(IReadOnlyCollection<ComparisonType> collection) =>
                this.collection = collection;

            public CollectionConstantPredicateBuilder<SourceType, ComparisonType> DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate) =>
                new CollectionConstantPredicateBuilder<SourceType, ComparisonType>(consecutiveItemBinaryExpressionFactory,
                    collection, sourceAndItemPredicate);
        }
    }
}
