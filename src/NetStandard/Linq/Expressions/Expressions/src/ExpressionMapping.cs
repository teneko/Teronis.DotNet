using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public readonly struct ExpressionMapping
    {
        public readonly Expression SourceTargetExpressionTools;
        public readonly Expression ReplacmentExpression;

        public ExpressionMapping(Expression source, Expression replacment)
        {
            SourceTargetExpressionTools = source;
            ReplacmentExpression = replacment;
        }
    }
}
