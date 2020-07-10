using System;
using System.Linq.Expressions;

namespace Teronis.Tools
{
    public static class ExpressionTools
    {
        public static Type getReturnType(LambdaExpression expression)
        {
            if ((expression.Body.NodeType == ExpressionType.Convert) ||
                (expression.Body.NodeType == ExpressionType.ConvertChecked)) {
                if (expression.Body is UnaryExpression unary) {
                    return unary.Operand.Type;
                }
            }

            return expression.Body.Type;
        }

        public static Type GetReturnType<T>(Expression<Func<T, object?>> expression) =>
            getReturnType(expression);

        public static Type GetReturnType(Expression<Func<object?>> expression) =>
            getReturnType(expression);

        public static string getReturnName(LambdaExpression expression)
        {
            if (!(expression.Body is MemberExpression body)) {
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
