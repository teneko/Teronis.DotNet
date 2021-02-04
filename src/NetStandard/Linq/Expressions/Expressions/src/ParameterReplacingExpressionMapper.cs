using System;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class ParameterReplacingExpressionMapper<SourceType, TargetType> : ExpressionMapper,
        IParameterReplacableExpressionMapper<SourceType, TargetType>
    {
        protected readonly ParameterExpression SourceParameterReplacement;
        protected readonly ParameterExpression TargetParameterReplacement;

        public ParameterReplacingExpressionMapper(ParameterExpression sourceParameterReplacement, ParameterExpression targetParameterReplacement)
        {
            SourceParameterReplacement = sourceParameterReplacement ?? throw new ArgumentNullException(nameof(sourceParameterReplacement));
            TargetParameterReplacement = targetParameterReplacement ?? throw new ArgumentNullException(nameof(targetParameterReplacement));
        }

        protected virtual T ReplaceExpressionParameters<T>(T expression, ParameterExpression from, ParameterExpression to)
            where T : Expression =>
            new ParameterReplacingVisitor(new[] { new SourceTargetPair<ParameterExpression, ParameterExpression>(from, to) })
            .VisitAndConvert(expression, nameof(ReplaceParameters));

        protected virtual void ReplaceParameters(ref Expression sourceBody, ParameterExpression sourceBodyParameter,
            ref Expression replacmentBody, ParameterExpression replacmentBodyParameter)
        {
            sourceBody = ReplaceExpressionParameters(sourceBody, sourceBodyParameter, SourceParameterReplacement);
            replacmentBody = ReplaceExpressionParameters(replacmentBody, replacmentBodyParameter, TargetParameterReplacement);
        }

        public void MapBodyAndParams<SourcePropertyType, TargetPropertyType>(Expression<Func<SourceType, SourcePropertyType>> source,
            Expression<Func<TargetType, TargetPropertyType>> replacment)
        {
            Expression sourceBody = source.Body;
            Expression replacmentBody = replacment.Body;
            ReplaceParameters(ref sourceBody, source.Parameters[0], ref replacmentBody, replacment.Parameters[0]);
            AddMapping(sourceBody, replacmentBody);
        }
    }
}
