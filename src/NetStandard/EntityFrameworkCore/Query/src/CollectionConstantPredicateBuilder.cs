using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public static class CollectionConstantPredicateBuilder<SourceType>
    {
        public static DeferredCreateBuilder<ComparisonType> FromComparisonList<ComparisonType>(
            IReadOnlyCollection<ComparisonType> comparisonList) =>
            new DeferredCreateBuilder<ComparisonType>(comparisonList);

        public readonly struct DeferredCreateBuilder<ComparisonType>
        {
            private readonly IReadOnlyCollection<ComparisonType> collection;

            public DeferredCreateBuilder(IReadOnlyCollection<ComparisonType> collection) =>
                this.collection = collection;

            public CollectionConstantPredicateBuilder<SourceType, ComparisonType> CreateBuilder(
                Func<Expression, Expression, BinaryExpression> valueBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceValuePredicate) =>
                new CollectionConstantPredicateBuilder<SourceType, ComparisonType>(valueBinaryExpressionFactory,
                    collection, sourceValuePredicate);
        }
    }
}
