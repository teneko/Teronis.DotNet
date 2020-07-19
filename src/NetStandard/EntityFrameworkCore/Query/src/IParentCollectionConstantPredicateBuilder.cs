using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    internal interface IParentCollectionConstantPredicateBuilder
    {
        bool IsRoot { get; }
        ParameterExpression SourceParameterExpression { get; }
        void StackBuilder(ICollectionConstantPredicateBuilder builder);
        bool TryPopBuilder([MaybeNullWhen(false)] out ICollectionConstantPredicateBuilder builder);
        void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpression);
    }
}
