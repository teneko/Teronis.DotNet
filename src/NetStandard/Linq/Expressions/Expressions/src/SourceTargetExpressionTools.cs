using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public static class SourceTargetExpressionTools
    {
        /// <summary>
        /// Reduces parameter list by replacing the expression parameter expression
        /// usages (of type <typeparamref name="T2"/>) with constant 
        /// expression crafted from <paramref name="constant"/>.
        /// </summary>
        /// <param name="constant">The value that will be used as constant.</param>
        /// <param name="lambdaWithParameters">
        /// The lambda expression from which the body will be rewritten.
        /// </param>
        /// <param name="positionalParameterReplacements">
        /// Positional parameters that replace parameters before they may get replaced by
        /// constants. 
        /// (E.g. replace parameter at index two of lambda parameters with specified parameter)
        /// </param>
        /// <returns>The body that got rewritten.</returns>
        public static Expression ReplaceParameterByConstantLambdaBody(
            LambdaExpression lambdaWithParameters,
            IReadOnlyDictionary<int, ParameterExpression>? positionalParameterReplacements,
            IReadOnlyDictionary<int, object?> positionalConstants,
            out IReadOnlyList<ParameterExpression> positionalParameters)
        {
            lambdaWithParameters = lambdaWithParameters ?? throw new ArgumentNullException(nameof(lambdaWithParameters));
            positionalConstants = positionalConstants ?? throw new ArgumentNullException(nameof(positionalConstants));

            var lambdaParameters = new List<ParameterExpression>(lambdaWithParameters.Parameters);
            Expression visitedLambdaBody;

            if (!(positionalParameterReplacements is null)) {
                var sourceTargetPairs = new SourceTargetPair<ParameterExpression, ParameterExpression>[positionalParameterReplacements.Count];
                var positionalParameterReplacementsEnumerator = positionalParameterReplacements.GetEnumerator();
                var sourceTargetPairsIndex = 0;

                while (positionalParameterReplacementsEnumerator.MoveNext()) {
                    var positionalParameterReplacementEnumeratorCurrent = positionalParameterReplacementsEnumerator.Current;

                    sourceTargetPairs[sourceTargetPairsIndex] = new SourceTargetPair<ParameterExpression, ParameterExpression>(
                        lambdaWithParameters.Parameters[positionalParameterReplacementEnumeratorCurrent.Key],
                        positionalParameterReplacementEnumeratorCurrent.Value);

                    sourceTargetPairsIndex++;
                }

                var parameterReplacer = new ParameterReplacingVisitor(sourceTargetPairs);
                visitedLambdaBody = parameterReplacer.Visit(lambdaWithParameters.Body);

                // After replacing parameter expression, you should update the references of lambda parameters.
                foreach (var replacedParameter in parameterReplacer.ReplacedSourceTargetPairs) {
                    var index = lambdaParameters.FindIndex(parameter => ReferenceEquals(parameter, replacedParameter.Source));

                    if (index >= 0) {
                        lambdaParameters[index] = replacedParameter.Target;
                    }
                }
            } else {
                visitedLambdaBody = lambdaWithParameters.Body;
            }

            var replacableNodes = new SourceTargetPair<Expression, Expression>[positionalConstants.Count];
            var positionalConstantsEnumerator = positionalConstants.GetEnumerator();
            var replacableNodesIndex = 0;

            while (positionalConstantsEnumerator.MoveNext()) {
                var positionalParameterReplacementEnumeratorCurrent = positionalConstantsEnumerator.Current;

                replacableNodes[replacableNodesIndex] = new SourceTargetPair<Expression, Expression>(
                    lambdaParameters[positionalParameterReplacementEnumeratorCurrent.Key],
                    Expression.Constant(positionalParameterReplacementEnumeratorCurrent.Value));

                replacableNodesIndex++;
            }

            var nodeReplacer = new NodeReplacerVisitor(replacableNodes);
            visitedLambdaBody = nodeReplacer.Visit(visitedLambdaBody);

            // After replacing parameters by constants, you should remove those lambda parameters.
            foreach (var replacedParameter in nodeReplacer.ReplacedSourceTargetPairs.Reverse()) {
                /* We assume that the front expressions got replaced first. */
                var index = lambdaParameters.FindIndex(parameter => ReferenceEquals(parameter, replacedParameter));

                if (index >= 0) {
                    lambdaParameters.RemoveAt(index);
                }
            }

            positionalParameters = lambdaParameters;
            return visitedLambdaBody;
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
        public static Expression<Func<SourceType, bool>> ReplaceParameterByConstantLambda<SourceType>(
            LambdaExpression lambdaWithParameters,
            IReadOnlyDictionary<int, ParameterExpression>? positionalParameterReplacements,
            IReadOnlyDictionary<int, object?> positionalConstants,
            out IReadOnlyList<ParameterExpression> positionalParameters)
        {
            var whereInConstantExpression = ReplaceParameterByConstantLambdaBody(
                lambdaWithParameters, 
                positionalParameterReplacements,
                positionalConstants,
                out positionalParameters);

            var newSourcePredicate = Expression.Lambda<Func<SourceType, bool>>(whereInConstantExpression, positionalParameters);
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
