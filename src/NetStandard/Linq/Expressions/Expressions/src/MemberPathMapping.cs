using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public readonly struct MemberPathMapping
    {
        public static MemberPathMapping Create(Expression from, Expression to)
        {
            var fromMemberStack = MemberPathEvaluator.EvaluateSourceMemberPath(from);
            var toMemberStack = MemberPathEvaluator.EvaluateSourceMemberPath(to);
            return new MemberPathMapping(fromMemberStack, toMemberStack);
        }

        public static MemberPathMapping Create<SourceType, TargetType>(Expression<Func<SourceType, object>> from,
            Expression<Func<TargetType, object>> to)
        {
            var fromMemberStack = MemberPathEvaluator.EvaluateSourceMemberPath(from.Body);
            var toMemberStack = MemberPathEvaluator.EvaluateSourceMemberPath(to.Body);
            return new MemberPathMapping(fromMemberStack, toMemberStack);
        }

        public readonly MemberPathEvaluation FromMemberPath { get; }
        public readonly MemberPathEvaluation ToMemberPath { get; }

        private MemberPathMapping(MemberPathEvaluation fromMemberPath, MemberPathEvaluation toMemberPath)
        {
            if (fromMemberPath.Equals(MemberPathEvaluation.Uninitialized)) {
                throw new ArgumentNullException(nameof(fromMemberPath));
            }

            FromMemberPath = fromMemberPath;

            if (toMemberPath.Equals(MemberPathEvaluation.Uninitialized)) {
                throw new ArgumentNullException(nameof(toMemberPath));
            }

            ToMemberPath = toMemberPath;
        }
    }
}
