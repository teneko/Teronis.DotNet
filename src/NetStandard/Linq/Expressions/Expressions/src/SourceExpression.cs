using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class SourceExpression
    {
        /// <summary>
        /// Reduces parameter list by replacing the expression parameter expression
        /// usages (of type <typeparamref name="ComparisonType"/>) with constant 
        /// expression crafted from <paramref name="comparisonValue"/>.
        /// </summary>
        /// <typeparam name="SourceType">The first parameter expression.</typeparam>
        /// <typeparam name="ComparisonType">The second parameter expression.</typeparam>
        /// <param name="comparisonValue">The value that will be used as constant.</param>
        /// <param name="sourceAndValuePredicate">The lambda expression from which the body will be rewritten.</param>
        /// <returns>The body that got rewritten.</returns>
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

            var parameterReplacer = new ParameterReplacerVisitor(new[] { sourceAndValuePredicate.Parameters[0] },
                new[] { sourceParameter });

            newSourcePredicateBody = parameterReplacer.Visit(newSourcePredicateBody);
            return newSourcePredicateBody;
        }

        #region

        /// <summary>
        /// Reduces parameter list by replacing the expression parameter expression
        /// usages (of type <typeparamref name="ComparisonType"/>) with constant 
        /// expression crafted from <paramref name="comparisonValue"/>.
        /// </summary>
        /// <typeparam name="SourceType">The first parameter expression.</typeparam>
        /// <typeparam name="ComparisonType">The second parameter expression.</typeparam>
        /// <param name="comparisonValue">The value that will be used as constant.</param>
        /// <param name="sourceAndValuePredicate">The lambda expression from which the body will be rewritten.</param>
        /// <returns>A new lambda with rewritten body.</returns>
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
        /// Replaces all expressions by those expressions who are defined 
        /// with the help of <paramref name="configureMemberMappings"/>.
        /// </summary>
        /// <typeparam name="SourceType">The source type.</typeparam>
        /// <typeparam name="TargetType">The target type.</typeparam>
        /// <param name="expression">The expression which may expressions</param>
        /// <param name="sourceParameter">The source parameter expression who may 
        /// used by those expressions you want to have replaced.</param>
        /// <param name="configureMemberMappings">Configures the expression 
        /// mappings.</param>
        /// <param name="targetParameter">The new target expression parameter that
        /// is used in replacement of the source target expression.</param>
        /// <returns>The expression with replaced expressions.</returns>
        public static Expression ReplaceExpressions<SourceType, TargetType>(Expression expression, ParameterExpression sourceParameter,
            ref ParameterExpression? targetParameter, Action<IParameterReplacableExpressionMapper<SourceType, TargetType>> configureMemberMappings)
        {
            targetParameter ??= Expression.Parameter(typeof(TargetType), "targetAsSource");
            var memberMappingBuilder = new ParameterReplacingExpressionMapper<SourceType, TargetType>(sourceParameter, targetParameter);
            configureMemberMappings(memberMappingBuilder);
            var memberMappings = memberMappingBuilder.GetMappings().ToList();

            if (memberMappings == null || memberMappings.Count == 0) {
                return expression;
            }

            var memberPathReplacer = new EqualityComparingExpressionReplacerVisitor(memberMappings);
            expression = memberPathReplacer.Visit(expression);
            return expression;
        }

        /// <summary>
        /// Replaces all expressions by those expressions who are defined 
        /// with the help of <paramref name="configureMemberMappings"/>.
        /// </summary>
        /// <typeparam name="SourceType">The source type.</typeparam>
        /// <typeparam name="TargetType">The target type.</typeparam>
        /// <param name="expression">The expression which may expressions</param>
        /// <param name="sourceParameter">The source parameter expression who may 
        /// used by those expressions you want to have replaced.</param>
        /// <param name="configureMemberMappings">Configures the expression 
        /// mappings.</param>
        /// <param name="targetParameter">The new target expression parameter that
        /// is used in replacement of the source target expression.</param>
        /// <returns>The expression with replaced expressions.</returns>
        public static Expression ReplaceExpressions<SourceType, TargetType>(Expression expression, ParameterExpression sourceParameter,
            Action<IParameterReplacableExpressionMapper<SourceType, TargetType>> configureMemberMappings,
            out ParameterExpression targetParameter)
        {
            targetParameter = null!;
            return ReplaceExpressions(expression, sourceParameter, ref targetParameter!, configureMemberMappings);
        }

        /// <summary>
        /// Replaces all expressions by those expressions who are defined 
        /// with the help of <paramref name="configureMemberMappings"/>.
        /// </summary>
        /// <typeparam name="SourceType">The source type.</typeparam>
        /// <typeparam name="TargetType">The target type.</typeparam>
        /// <param name="expression">The expression which may expressions</param>
        /// <param name="sourceParameter">The source parameter expression who may 
        /// used by those expressions you want to have replaced.</param>
        /// <param name="configureMemberMappings">Configures the expression 
        /// mappings.</param>
        /// <param name="targetParameter">The new target expression parameter that
        /// is used in replacement of the source target expression.</param>
        /// <returns>The expression with replaced expressions.</returns>
        public static Expression ReplaceParameters<SourceType, TargetType>(Expression expression, ParameterExpression sourceParameter,
            ParameterExpression targetParameter, Action<IParameterReplacableExpressionMapper<SourceType, TargetType>> configureMemberMappings)
        {
            targetParameter = targetParameter ?? throw new ArgumentNullException(nameof(targetParameter));
            return ReplaceExpressions(expression, sourceParameter, ref targetParameter!, configureMemberMappings);
        }

        #endregion
    }
}
