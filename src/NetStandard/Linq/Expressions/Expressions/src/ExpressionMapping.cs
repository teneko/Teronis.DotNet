using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public readonly struct ExpressionMapping
    {
        public readonly Expression SourceExpression;
        public readonly Expression ReplacmentExpression;

        public ExpressionMapping(Expression source, Expression replacment)
        {
            SourceExpression = source;
            ReplacmentExpression = replacment;
        }
    }
}
