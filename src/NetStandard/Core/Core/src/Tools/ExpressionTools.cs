using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Teronis.Tools
{
    public static class ExpressionTools
    {
        public static Type getReturnType(LambdaExpression expression)
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

        public static Type GetReturnType<T>(Expression<Func<T, object?>> expression) =>
            getReturnType(expression);

        public static Type GetReturnType(Expression<Func<object?>> expression) =>
            getReturnType(expression);

        public static string getReturnName(LambdaExpression expression) {
            MemberExpression? body = expression.Body as MemberExpression;

            if (body == null) {
                UnaryExpression ubody = (UnaryExpression)expression.Body;

                body = ubody.Operand as MemberExpression ??
                    throw new ArgumentException("Expression body is null");
            }

            return body.Member.Name;
        }

        public static string GetReturnName<T>(Expression<Func<T, object?>> expression) =>
            getReturnName(expression);

        public static string GetReturnName(Expression<Func<object?>> expression) =>
            getReturnName(expression);
    }
}
