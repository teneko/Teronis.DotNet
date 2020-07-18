using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public interface ICollectionConstantPredicateBuilder<SourceType, ComparisonType>
    {
        ICollectionConstantPredicateBuilder<SourceType, ComparisonType> ThenWhereInCollectionConstant<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValue,
            Func<Expression, Expression, BinaryExpression> valueBinaryExpressionFactory,
            Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate,
            Action<ICollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate = null);
    }
}
