// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class ExpressionTools
    {
        public static Type GetReturnType(LambdaExpression expression)
        {
            if (expression.Body.NodeType == ExpressionType.Convert ||
                expression.Body.NodeType == ExpressionType.ConvertChecked) {
                if (expression.Body is UnaryExpression unary) {
                    return unary.Operand.Type;
                }
            }

            return expression.Body.Type;
        }

        public static string GetReturnName(LambdaExpression expression)
        {
            if (!(expression.Body is MemberExpression body)) {
                UnaryExpression ubody = (UnaryExpression)expression.Body;

                body = ubody.Operand as MemberExpression ??
                    throw new ArgumentException("Expression body is null");
            }

            return body.Member.Name;
        }

        /// <summary>
        /// Gets the member names of an anonymous type.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The member names.</returns>
        /// <exception cref="ArgumentException">Expression body is not of type <see cref="NewExpression"/>.</exception>
        public static string[] GetAnonymousTypeNames(LambdaExpression expression)
        {
            if (!(expression.Body is NewExpression newExpression)) {
                throw new ArgumentException($"Expression body is not of type {nameof(NewExpression)}.");
            }

            if (newExpression.Members is null) {
                return new string[0];
            }

            var membersCount = newExpression.Members.Count;
            var names = new string[membersCount];

            for (var index = 0; index < membersCount; index++) {
                names[index] = newExpression.Members[index].Name;
            }

            return names;
        }
    }
}
