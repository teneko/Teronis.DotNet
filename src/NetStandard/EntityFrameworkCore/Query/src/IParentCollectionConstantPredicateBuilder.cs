using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    internal interface IParentCollectionConstantPredicateBuilder
    {
        bool IsRoot { get; }
        ParameterExpression SourceParameterExpression { get; }
        void StackBuilder(IChildCollectionConstantPredicateBuilder builder);
        bool TryPopBuilder([MaybeNullWhen(false)] out IChildCollectionConstantPredicateBuilder builder);
        void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpression);
    }
}
