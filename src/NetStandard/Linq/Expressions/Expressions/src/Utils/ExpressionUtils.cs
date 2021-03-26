// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;

namespace Teronis.Linq.Expressions.Utils
{
    public class ExpressionTools
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
