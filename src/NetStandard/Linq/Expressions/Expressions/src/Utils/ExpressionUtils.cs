using System.Linq.Expressions;

namespace Teronis.Linq.Expressions.Utilities
{
    public class ExpressionUtils
    {
        public static MemberExpression? TryGetMember(Expression? expression)
        {
            while (!(expression is null)) {
                if (expression is MemberExpression member) {
                    return member;
                } else if (expression is UnaryExpression unary) {
                    expression = unary.Operand;
                    continue;
                }

                expression = null;
            }

            return null;
        }
    }
}
