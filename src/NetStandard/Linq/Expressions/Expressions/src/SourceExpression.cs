using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        /// <summary>
        /// Replaces all member accesses with <typeparamref name="SourceType"/> parameter expression as origin
        /// by those member accesses who are defined with help of <paramref name="configureMemberMappings"/>.
        /// </summary>
        /// <typeparam name="SourceType">Source type.</typeparam>
        /// <typeparam name="TargetType">Target type.</typeparam>
        /// <param name="expression">The expression which may contain member accesses.</param>
        /// <param name="sourceParameter">The source parameter expression who is used by those member
        /// accesses you want to have replaced.</param>
        /// <param name="configureMemberMappings">Lets you configures the member mappings.</param>
        /// <param name="targetParameter">The new target expression parameter that is used in replacement of the
        /// source target expression</param>
        /// <returns>The expression with replaced member accesses.</returns>
        public static Expression ReplaceParameter<SourceType, TargetType>(Expression expression, ParameterExpression sourceParameter,
            ref ParameterExpression? targetParameter, Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings)
        {
            targetParameter = targetParameter ?? Expression.Parameter(typeof(TargetType), "targetAsSource");
            var memberMappingBuilder = new NodeReplacingMemberMappingBuilder<SourceType, TargetType>(sourceParameter, targetParameter);
            configureMemberMappings(memberMappingBuilder);
            var memberMappings = memberMappingBuilder.GetMappings().ToList();

            if (memberMappings == null || memberMappings.Count == 0) {
                return expression;
            }

            var memberPathReplacer = new SourceMemberPathReplacerVisitor(memberMappings);
            expression = memberPathReplacer.Visit(expression);
            return expression;
        }

        public static Expression ReplaceParameter<SourceType, TargetType>(Expression expression, ParameterExpression sourceParameter,
            Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings,
            out ParameterExpression targetParameter)
        {
            targetParameter = null!;
            return ReplaceParameter(expression, sourceParameter, ref targetParameter!, configureMemberMappings);
        }

        public static Expression ReplaceParameter<SourceType, TargetType>(Expression expression, ParameterExpression sourceParameter,
            ParameterExpression targetParameter, Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings)
        {
            targetParameter = targetParameter ?? throw new ArgumentNullException(nameof(targetParameter));
            return ReplaceParameter(expression, sourceParameter, ref targetParameter!, configureMemberMappings);
        }
    }
}
