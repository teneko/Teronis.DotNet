using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public readonly struct MemberPathMapping
    {
        internal static void ThrowOnNonMemberBody(Expression? expressionBody, string parameterName)
        {
            if (!(expressionBody is MemberExpression)) {
                throw new ArgumentNullException(parameterName, $"Expression body is not of type {nameof(MemberExpression)}");
            }
        }

        /// <summary>
        /// Creates a member path mapping between <paramref name="from"/>
        /// and <paramref name="to"/> with containing path evaluation.
        /// </summary>
        /// <param name="from">A member expression you are mapping from.</param>
        /// <param name="to">A member expression you are mapping to.</param>
        /// <returns>Member path mapping.</returns>
        public static MemberPathMapping Create(MemberExpression from, MemberExpression to)
        {
            var fromMemberStack = MemberPathEvaluator.EvaluateMemberPath(from);
            var toMemberStack = MemberPathEvaluator.EvaluateMemberPath(to);
            return new MemberPathMapping(fromMemberStack, toMemberStack);
        }

        /// <summary>
        /// Creates a member path mapping between <paramref name="from"/>
        /// and <paramref name="to"/> with containing path evaluation.
        /// </summary>
        /// <param name="from">A lambda expression with member access expression (a => a.b) as body.</param>
        /// <param name="to">A lambda expression with member access expression (a => a.b) as body.</param>
        /// <returns>Member path mapping.</returns>
        public static MemberPathMapping Create<SourceType, TargetType>(Expression<Func<SourceType, object?>> from,
            Expression<Func<TargetType, object?>> to)
        {
            ThrowOnNonMemberBody(from?.Body, nameof(from));
            ThrowOnNonMemberBody(to?.Body, nameof(to));
            var fromMemberStack = MemberPathEvaluator.EvaluateMemberPath((MemberExpression)from!.Body);
            var toMemberStack = MemberPathEvaluator.EvaluateMemberPath((MemberExpression)to!.Body);
            return new MemberPathMapping(fromMemberStack, toMemberStack);
        }

        public readonly MemberPathEvaluation FromPathEvaluation { get; }
        public readonly MemberPathEvaluation ToPathEvaluation { get; }

        private MemberPathMapping(MemberPathEvaluation fromPathEvaluation, MemberPathEvaluation toPathEvaluation)
        {
            if (fromPathEvaluation.Equals(default)) {
                throw new ArgumentNullException(nameof(fromPathEvaluation));
            }

            FromPathEvaluation = fromPathEvaluation;

            if (toPathEvaluation.Equals(default)) {
                throw new ArgumentNullException(nameof(toPathEvaluation));
            }

            ToPathEvaluation = toPathEvaluation;
        }
    }
}
