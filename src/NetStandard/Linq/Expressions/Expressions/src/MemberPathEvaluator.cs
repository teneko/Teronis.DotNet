using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class MemberPathEvaluator
    {
        private static readonly MemberPathEvaluatorVisitor @default;

        static MemberPathEvaluator()
        {
            @default = new MemberPathEvaluatorVisitor();
        }

        /// <summary>
        /// Evalutes path of member upwards <see cref="MemberExpression.Expression"/>.
        /// It uses default instance of <see cref="MemberPathEvaluatorVisitor"/> and is
        /// therefore not thread-safe.
        /// </summary>
        /// <param name="memberNode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Source node is no constant or parameter.</exception>
        /// <exception cref="ArgumentException">Source node is no constant or parameter.</exception>
        public static MemberPathEvaluation EvaluateMemberPath(MemberExpression memberNode) =>
            @default.EvaluateMemberPath(memberNode);
    }
}
