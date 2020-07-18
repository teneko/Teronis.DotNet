using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class MemberPathEvaluationExtensions
    {
        public static bool HasOnlySource(this MemberPathEvaluation evaluation) =>
            !evaluation.HasHighMemberExpression && evaluation.HasSourceExpression;

        public static Expression GetHighestExpressionOrException(this MemberPathEvaluation pathEvaluation) =>
            pathEvaluation.GetHighestExpression() ?? throw new ArgumentException("Path evaluation does not have either source or member expression.");
    }
}
