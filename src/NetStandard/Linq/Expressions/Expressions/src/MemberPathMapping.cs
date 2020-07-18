using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public readonly struct MemberPathMapping
    {
        internal static void ThrowOnNonMemberBody<T>(Expression<Func<T, object?>> expression, string parameterName)
        {
            if (!(expression?.Body is MemberExpression)) {
                throw new ArgumentNullException(parameterName, $"Expression body is not of type {nameof(MemberExpression)}");
            }
        }

        public static MemberPathMapping Create(MemberExpression from, MemberExpression to)
        {
            var fromMemberStack = MemberPathEvaluator.EvaluateMemberPath(from);
            var toMemberStack = MemberPathEvaluator.EvaluateMemberPath(to);
            return new MemberPathMapping(fromMemberStack, toMemberStack);
        }

        public static MemberPathMapping Create<SourceType, TargetType>(Expression<Func<SourceType, object>> from,
            Expression<Func<TargetType, object>> to)
        {
            ThrowOnNonMemberBody(from, nameof(from));
            ThrowOnNonMemberBody(to, nameof(to));
            var fromMemberStack = MemberPathEvaluator.EvaluateMemberPath((MemberExpression)from.Body);
            var toMemberStack = MemberPathEvaluator.EvaluateMemberPath((MemberExpression)to.Body);
            return new MemberPathMapping(fromMemberStack, toMemberStack);
        }

        public readonly MemberPathEvaluation FromMemberPath { get; }
        public readonly MemberPathEvaluation ToMemberPath { get; }

        private MemberPathMapping(MemberPathEvaluation fromMemberPath, MemberPathEvaluation toMemberPath)
        {
            if (fromMemberPath.Equals(default)) {
                throw new ArgumentNullException(nameof(fromMemberPath));
            }

            FromMemberPath = fromMemberPath;

            if (toMemberPath.Equals(default)) {
                throw new ArgumentNullException(nameof(toMemberPath));
            }

            ToMemberPath = toMemberPath;
        }
    }
}
