using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class SourceMemberPathReplacerVisitor : ExpressionVisitor
    {
        public IReadOnlyCollection<MemberPathMapping> memberMappings;

        public SourceMemberPathReplacerVisitor(IReadOnlyCollection<MemberPathMapping> memberMappings) =>
            this.memberMappings = memberMappings;

        protected override Expression VisitMember(MemberExpression node)
        {
            var memberPath = MemberPathEvaluator.EvaluateMemberPath(node);

            foreach (var memberMapping in memberMappings) {
                var otherFromMemberPath = memberMapping.FromMemberPath;

                if (!otherFromMemberPath.HasHighMemberExpression) {
                    continue;
                }

                if (memberPath.Equals(otherFromMemberPath)) {
                    return memberMapping.ToMemberPath.GetHighestExpressionOrException();
                }
            }

            return base.VisitMember(node);
        }

        private bool tryVisitSource<T>(T fromNode, [MaybeNullWhen(false)] out Expression result)
        {
            foreach (var memberMapping in memberMappings) {
                var otherFromMemberPathEvaluation = memberMapping.FromMemberPath;

                if (!otherFromMemberPathEvaluation.HasOnlySource()) {
                    continue;
                }

                if (otherFromMemberPathEvaluation.SourceExpression is T otherFromParamaterNode
                    && EqualityComparer<T>.Default.Equals(fromNode, otherFromParamaterNode)) {
                    result = otherFromMemberPathEvaluation.GetHighestExpressionOrException();
                    return true;
                }
            }

            result = null;
            return false;
        }

        protected override Expression VisitParameter(ParameterExpression fromParameterNode)
        {
            if (tryVisitSource(fromParameterNode, out var result)) {
                return result;
            }

            return base.VisitParameter(fromParameterNode);
        }

        protected override Expression VisitConstant(ConstantExpression fromConstantNode)
        {
            if (tryVisitSource(fromConstantNode, out var result)) {
                return result;
            }

            return base.VisitConstant(fromConstantNode);
        }
    }
}
