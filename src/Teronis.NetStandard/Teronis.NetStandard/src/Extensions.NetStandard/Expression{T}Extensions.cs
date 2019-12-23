using System;
using System.Linq.Expressions;

namespace Teronis.Extensions.NetStandard
{
    public static class ExpressionExtensions
    {
        public static string GetReturnName(Expression<Func<object>> exp)
            => (exp.Body as MemberExpression ?? ((UnaryExpression)exp.Body).Operand as MemberExpression)?.Member.Name;
    }
}
