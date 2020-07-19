using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    internal class NodeReplacingMemberMappingBuilder<SourceType, TargetType> : TypedSourceTargetMemberMapper<SourceType, TargetType>
    {
        private readonly ParameterExpression sourceParameterReplacement;
        private readonly ParameterExpression targetParameterReplacement;

        public NodeReplacingMemberMappingBuilder(ParameterExpression sourceParameterReplacement, ParameterExpression targetParameterReplacement)
        {
            this.sourceParameterReplacement = sourceParameterReplacement ?? throw new ArgumentNullException(nameof(sourceParameterReplacement));
            this.targetParameterReplacement = targetParameterReplacement ?? throw new ArgumentNullException(nameof(targetParameterReplacement));
        }

        private MemberExpression replaceParameter(MemberExpression body, ParameterExpression from, ParameterExpression to) =>
            new ParameterReplacerVisitor(new[] { from }, new[] { to }).VisitAndConvert(body, nameof(replaceParameter));

        protected override MemberExpression GetBody(Expression<Func<SourceType, object?>> from) =>
            replaceParameter((MemberExpression)from.Body, from.Parameters[0], sourceParameterReplacement);

        protected override MemberExpression GetBody(Expression<Func<TargetType, object?>> to) =>
            replaceParameter((MemberExpression)to.Body, to.Parameters[0], targetParameterReplacement);
    }
}
