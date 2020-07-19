using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class SourceExpression
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="SourceType"></typeparam>
        /// <typeparam name="ComparisonType"></typeparam>
        /// <param name="comparisonValue"></param>
        /// <param name="sourceAndValuePredicate"></param>
        /// <returns></returns>
        public static Expression WhereInConstant<SourceType, ComparisonType>(
            [AllowNull] ComparisonType comparisonValue, Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndValuePredicate,
            out ParameterExpression sourceParameter, ParameterExpression? sourceParameterReplacement = null)
        {
            sourceAndValuePredicate = sourceAndValuePredicate ?? throw new ArgumentNullException(nameof(sourceAndValuePredicate));

            /* Replace KeyType parameter/member by constant. */
            var comparisonParameter = sourceAndValuePredicate.Parameters[1];
            var comparisonConstant = Expression.Constant(comparisonValue, comparisonParameter.Type);

            var nodeReplacer = new NodeReplacerVisitor(comparisonParameter, comparisonConstant);
            var newSourcePredicateBody = nodeReplacer.Visit(sourceAndValuePredicate.Body);

            /* Replace source parameter by known one. */
            sourceParameter = sourceParameterReplacement ?? sourceAndValuePredicate.Parameters[0];

            var parameterExchanger = new ParameterReplacerVisitor(new[] { sourceAndValuePredicate.Parameters[0] },
                new[] { sourceParameter });

            newSourcePredicateBody = parameterExchanger.VisitAndConvert(newSourcePredicateBody, nameof(WhereInConstant));

            return newSourcePredicateBody;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="SourceType"></typeparam>
        /// <typeparam name="ComparisonType"></typeparam>
        /// <param name="comparisonValue"></param>
        /// <param name="sourceAndValuePredicate"></param>
        /// <param name="sourceParameterExpression">If parameter needs to be supplied from outside.</param>
        /// <returns></returns>
        public static Expression<Func<SourceType, bool>> WhereInConstantLambda<SourceType, ComparisonType>(
            [AllowNull] ComparisonType comparisonValue, Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndValuePredicate,
            ParameterExpression? sourceParameterExpression = null)
        {
            sourceAndValuePredicate = sourceAndValuePredicate ?? throw new ArgumentNullException(nameof(sourceAndValuePredicate));
            sourceParameterExpression ??= Expression.Parameter(typeof(SourceType), "source");

            /* Replace KeyType parameter/member by constant. */
            var whereInConstantExpression = WhereInConstant(comparisonValue, sourceAndValuePredicate, out _, sourceParameterReplacement: sourceParameterExpression);
            var newSourcePredicate = Expression.Lambda<Func<SourceType, bool>>(whereInConstantExpression, sourceAndValuePredicate.Parameters[0]);

            return newSourcePredicate;
        }
    }
}
