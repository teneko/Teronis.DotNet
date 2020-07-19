using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public interface IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType>
    {
        CollectionConstantPredicateBuilder<SourceType, ComparisonType>.IDeferredThenCreateCollectionConstantBuilder<ThenComparisonType> ThenInCollection<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues);
    }
}
