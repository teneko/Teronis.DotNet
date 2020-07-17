using System;
using System.Collections.Generic;
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
        /// <param name="sourcePredicate"></param>
        /// <returns></returns>
        public static Expression WhereInConstant<SourceType, ComparisonType>(
            [AllowNull]ComparisonType comparisonValue, Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourcePredicate,
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
        /// <param name="lambdaConstantValue">Used when list is null. Default is false.</param>
        /// <param name="sourceParameterExpression">If parameter needs to be supplied from outside.</param>
        /// <returns></returns>
        public static Expression<Func<SourceType, bool>> WhereInConstantLambda<SourceType, ComparisonType>(Expression whereInConstantExpression,
            ParameterExpression sourceParameterExpression)
        {
            whereInConstantExpression = whereInConstantExpression ?? throw new ArgumentNullException(nameof(whereInConstantExpression));
            sourceParameterExpression = sourceParameterExpression ?? throw new ArgumentNullException(nameof(sourceParameterExpression));
            var newSourcePredicate = Expression.Lambda<Func<SourceType, bool>>(whereInConstantExpression, sourceParameterExpression);
            return newSourcePredicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="SourceType"></typeparam>
        /// <typeparam name="ComparisonType"></typeparam>
        /// <param name="comparisonValue"></param>
        /// <param name="sourcePredicate"></param>
        /// <param name="lambdaConstantValue">Used when list is null. Default is false.</param>
        /// <param name="sourceParameterExpression">If parameter needs to be supplied from outside.</param>
        /// <returns></returns>
        public static Expression<Func<SourceType, bool>> WhereInConstantLambda<SourceType, ComparisonType>(
            [AllowNull]ComparisonType comparisonValue, Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourcePredicate,
            ParameterExpression? sourceParameterExpression = null)
        {
            sourcePredicate = sourcePredicate ?? throw new ArgumentNullException(nameof(sourcePredicate));
            sourceParameterExpression = sourceParameterExpression ?? Expression.Parameter(typeof(SourceType), "source");

            /* Replace KeyType parameter/member by constant. */
            var whereInConstantExpression = WhereInConstant(comparisonValue, sourcePredicate, out _, sourceParameterReplacement: sourceParameterExpression);
            var newSourcePredicate = Expression.Lambda<Func<SourceType, bool>>(whereInConstantExpression, sourcePredicate.Parameters[0]);

            return newSourcePredicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="SourceType"></typeparam>
        /// <typeparam name="ComparisonType"></typeparam>
        /// <param name="comparisonList"></param>
        /// <param name="sourcePredicate"></param>
        /// <param name="binaryExpressionFactory">
        /// If null <see cref="Expression.OrElse(Expression, Expression)"/> will be used to concentrate the expression from each collection item.
        /// </param>
        /// <returns></returns>
        public static Expression<Func<SourceType, bool>> WhereInCollectionConstant<SourceType, ComparisonType>(
            IReadOnlyCollection<ComparisonType> comparisonList,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourcePredicate,
            Func<Expression, Expression, BinaryExpression>? binaryExpressionFactory = null)
        {
            if (comparisonList == null || comparisonList.Count == 0) {
                throw new ArgumentException("Comparison list cannot be null and needs at least one item.");
            }

            binaryExpressionFactory = binaryExpressionFactory ?? Expression.OrElse;

            Expression whereInConstantExpression(ComparisonType comparisonValue, out ParameterExpression parameter,
                ParameterExpression? parameterReplacement = null) =>
                WhereInConstant(comparisonValue, sourcePredicate, out parameter, sourceParameterReplacement: parameterReplacement);

            var aggregatedWhereInConstant = comparisonList.Skip(1).Aggregate(whereInConstantExpression(comparisonList.First(), out var sourceParameter),
                (aggreagation, comparisonValue) => binaryExpressionFactory(aggreagation, whereInConstantExpression(comparisonValue, out sourceParameter, sourceParameter)));


            var whereInConstantLambda = WhereInConstantLambda<SourceType, ComparisonType>(aggregatedWhereInConstant, sourceParameter);
            return whereInConstantLambda;
        }
    }
}
