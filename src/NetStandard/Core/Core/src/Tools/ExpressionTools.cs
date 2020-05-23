using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Teronis.Tools
{
    public static class ExpressionTools
    {
        public static Type GetReturnType<T>(Expression<Func<T, object>> expression)
        {
            if ((expression.Body.NodeType == ExpressionType.Convert) ||
                (expression.Body.NodeType == ExpressionType.ConvertChecked)) {
                var unary = expression.Body as UnaryExpression;

                if (unary != null) {
                    return unary.Operand.Type;
                }
            }

            return expression.Body.Type;
        }

        public static string GetReturnName<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;

            if (body == null) {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
