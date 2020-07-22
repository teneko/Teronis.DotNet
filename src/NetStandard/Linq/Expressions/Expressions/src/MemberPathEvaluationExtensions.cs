using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class MemberPathEvaluationExtensions
    {
        /// <summary>
        /// Returns a value that indicates whether only source expression is present.
        /// </summary>
        /// <param name="pathEvaluation">The member path evaluation.</param>
        /// <returns>True if <see cref="MemberPathEvaluation.HasAscendantMemberExpression"/> 
        /// is false and <see cref="MemberPathEvaluation.HasSourceExpression"/> is true.</returns>
        public static bool HasOnlySource(this MemberPathEvaluation pathEvaluation) =>
            !pathEvaluation.HasAscendantMemberExpression && pathEvaluation.HasSourceExpression;

        /// <summary>
        /// Gets the highest expression or an expcetion is thrown.
        /// </summary>
        /// <param name="pathEvaluation">The member path evaluation.</param>
        /// <returns>Highest expression in the member path.</returns>
        public static Expression GetMostAscendantMemberOrException(this MemberPathEvaluation pathEvaluation) =>
            pathEvaluation.GetMostAscendantExpression() ?? throw new ArgumentException("Path evaluation does not have either source or member expression.");
    }
}
