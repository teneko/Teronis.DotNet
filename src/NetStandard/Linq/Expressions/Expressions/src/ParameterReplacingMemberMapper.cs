using System;
using System.Linq.Expressions;
using Teronis.Linq.Expressions.Utils;

namespace Teronis.Linq.Expressions
{
    internal class ParameterReplacingMemberMapper<SourceType, TargetType> : TypedSourceTargetMemberMapper<SourceType, TargetType>
    {
        private readonly ParameterExpression sourceParameterReplacement;
        private readonly ParameterExpression targetParameterReplacement;

        public ParameterReplacingMemberMapper(ParameterExpression sourceParameterReplacement, ParameterExpression targetParameterReplacement)
        {
            this.sourceParameterReplacement = sourceParameterReplacement ?? throw new ArgumentNullException(nameof(sourceParameterReplacement));
            this.targetParameterReplacement = targetParameterReplacement ?? throw new ArgumentNullException(nameof(targetParameterReplacement));
        }

        private Exception createLambdaExpressionArgumentError(string paramName) =>
            new ArgumentException("Lambda expression does not contain a member expression as body.", paramName);

        private MemberExpression replaceParameter(MemberExpression body, ParameterExpression from, ParameterExpression to) =>
            new ParameterReplacerVisitor(new[] { from }, new[] { to }).VisitAndConvert(body, nameof(replaceParameter));

        protected override MemberExpression GetMemberMappingFrom(LambdaExpression from)
        {
            var body = ExpressionUtils.TryGetMember(from.Body) ?? throw createLambdaExpressionArgumentError(nameof(from));
            return replaceParameter(body, from.Parameters[0], sourceParameterReplacement);
        }

        protected override MemberExpression GetMemberMappingTo(LambdaExpression to)
        {
            var body = ExpressionUtils.TryGetMember(to.Body) ?? throw createLambdaExpressionArgumentError(nameof(to));
            return replaceParameter(body, to.Parameters[0], targetParameterReplacement);
        }
    }
}
