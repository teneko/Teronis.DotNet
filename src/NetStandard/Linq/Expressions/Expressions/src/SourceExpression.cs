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
        /// <param name="sourcePredicate"></param>
        /// <returns></returns>
        public static Expression WhereInConstant<SourceType, ComparisonType>(
            [AllowNull] ComparisonType comparisonValue, Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourcePredicate,
            out ParameterExpression sourceParameter, ParameterExpression? sourceParameterReplacement = null)
        {
            sourcePredicate = sourcePredicate ?? throw new ArgumentNullException(nameof(sourcePredicate));

            /* Replace KeyType parameter/member by constant. */
            var comparisonParameter = sourcePredicate.Parameters[1];
            var comparisonConstant = Expression.Constant(comparisonValue, comparisonParameter.Type);

            var nodeReplacer = new NodeReplacerVisitor(comparisonParameter, comparisonConstant);
            var newSourcePredicateBody = nodeReplacer.Visit(sourcePredicate.Body);

            /* Replace source parameter by known one. */
            sourceParameter = sourceParameterReplacement ?? sourcePredicate.Parameters[0];

            var parameterExchanger = new ParameterReplacerVisitor(new[] { sourcePredicate.Parameters[0] },
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
        /// <param name="sourcePredicate"></param>
        /// <param name="sourceParameterExpression">If parameter needs to be supplied from outside.</param>
        /// <returns></returns>
        public static Expression<Func<SourceType, bool>> WhereInConstantLambda<SourceType, ComparisonType>(
            [AllowNull] ComparisonType comparisonValue, Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourcePredicate,
            ParameterExpression? sourceParameterExpression = null)
        {
            sourcePredicate = sourcePredicate ?? throw new ArgumentNullException(nameof(sourcePredicate));
            sourceParameterExpression ??= Expression.Parameter(typeof(SourceType), "source");

            /* Replace KeyType parameter/member by constant. */
            var whereInConstantExpression = WhereInConstant(comparisonValue, sourcePredicate, out _, sourceParameterReplacement: sourceParameterExpression);
            var newSourcePredicate = Expression.Lambda<Func<SourceType, bool>>(whereInConstantExpression, sourcePredicate.Parameters[0]);

            return newSourcePredicate;
        }
    }
}
