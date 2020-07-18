using System;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    internal class NodeReplacingMemberMappingBuilder<SourceType, TargetType> : TypedSourceTargetMemberMappper<SourceType, TargetType>
    {
        private readonly ParameterExpression sourceParameterReplacement;
        private readonly ParameterExpression targetParameterReplacement;

        public NodeReplacingMemberMappingBuilder(ParameterExpression sourceParameterReplacement, ParameterExpression targetParameterReplacement)
        {
            this.sourceParameterReplacement = sourceParameterReplacement ?? throw new ArgumentNullException(nameof(sourceParameterReplacement));
            this.targetParameterReplacement = targetParameterReplacement ?? throw new ArgumentNullException(nameof(targetParameterReplacement));
        }

        private Expression replaceParameter(Expression body, ParameterExpression from, ParameterExpression to) =>
            new ParameterReplacerVisitor(new[] { from }, new[] { to }).Visit(body);

        protected override Expression GetBody(Expression<Func<SourceType, object>> from) =>
            replaceParameter(from.Body, from.Parameters[0], sourceParameterReplacement);

        protected override Expression GetBody(Expression<Func<TargetType, object>> to) =>
            replaceParameter(to.Body, to.Parameters[0], targetParameterReplacement);
    }
}
