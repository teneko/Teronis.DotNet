using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Teronis.Linq.Expressions
{
    public class CollectionConstantPredicateBuilder<SourceType, ComparisonType> : ICollectionConstantPredicateBuilder<SourceType, ComparisonType>, ICollectionConstantPredicateBuilder
    {
        private IParentCollectionConstantPredicateBuilder parentBuilder;
        private List<SourceValueComparison<ComparisonType>> sourceValueComparisons = null!;
        private readonly Func<Expression, Expression, BinaryExpression> valueBinaryExpressionFactory;
        private readonly Func<Expression, Expression, BinaryExpression>? parentBinaryExpressionFactory;

        public CollectionConstantPredicateBuilder(
            Func<Expression, Expression, BinaryExpression> valueBinaryExpressionFactory,
            IReadOnlyCollection<ComparisonType> comparisonList,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceValuePredicate)
        {
            if (comparisonList is null || comparisonList.Count == 0) {
                throw new ArgumentNullException(nameof(comparisonList));
            }

            this.valueBinaryExpressionFactory = valueBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(valueBinaryExpressionFactory));
            onConstruction(comparisonList, sourceValuePredicate, out var sourceParameterExpression, null);
            parentBuilder = new RootBuilder(new Stack<ICollectionConstantPredicateBuilder>(), sourceParameterExpression);
        }

        private CollectionConstantPredicateBuilder(IParentCollectionConstantPredicateBuilder parentBuilder,
            IReadOnlyCollection<ComparisonType> comparisonList,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceValuePredicate,
            Func<Expression, Expression, BinaryExpression> valueBinaryExpressionFactory,
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory)
        {
            this.parentBuilder = parentBuilder ?? throw new ArgumentNullException(nameof(parentBuilder));
            comparisonList = comparisonList ?? throw new ArgumentNullException(nameof(comparisonList));
            sourceValuePredicate = sourceValuePredicate ?? throw new ArgumentNullException(nameof(sourceValuePredicate));
            this.valueBinaryExpressionFactory = valueBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(valueBinaryExpressionFactory));
            this.parentBinaryExpressionFactory = parentBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(parentBinaryExpressionFactory));
            onConstruction(comparisonList, sourceValuePredicate, out _, parentBuilder.SourceParameterExpression);
            // We only want to build level one and upward levels.
            parentBuilder.StackBuilder(this);
        }

        private void onConstruction(IReadOnlyCollection<ComparisonType> comparisonList,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceValuePredicate,
            out ParameterExpression sourceParameterExpression, ParameterExpression? sourceParameterReplacement)
        {
            sourceParameterExpression = null!;
            sourceValueComparisons = new List<SourceValueComparison<ComparisonType>>();
            var comparisonValueEnumerator = comparisonList.GetEnumerator();

            while (comparisonValueEnumerator.MoveNext()) {
                var comparisonValue = comparisonValueEnumerator.Current;

                var whereInConstantExpression = SourceExpression.WhereInConstant(comparisonValue, sourceValuePredicate,
                    out sourceParameterExpression, sourceParameterReplacement: sourceParameterReplacement);

                var sourceValueComparison = new SourceValueComparison<ComparisonType>(comparisonValue, whereInConstantExpression);
                sourceValueComparisons.Add(sourceValueComparison);
            }
        }

        public CollectionConstantPredicateBuilder<SourceType, ComparisonType> ThenWhereInCollectionConstant<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValue,
            Func<Expression, Expression, BinaryExpression> valueBinaryExpressionFactory,
            Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate,
            Action<ICollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate = null)
        {
            var sourceValueComparisonsCount = sourceValueComparisons.Count;

            for (int comparisonIndex = 0; comparisonIndex < sourceValueComparisonsCount; comparisonIndex++) {
                var sourceValueComparison = sourceValueComparisons[comparisonIndex];
                var list = getComparisonValue(sourceValueComparison.ComparisonValue);

                if (list is null || list.Count == 0) {
                    continue;
                }

                var expressionAppender = new ExpressionAppender(comparisonIndex, sourceValueComparisons);
                var parentBuilder = new ParentBuilder(this, expressionAppender);

                var builder = new CollectionConstantPredicateBuilder<SourceType, ThenComparisonType>(parentBuilder, list, sourcePredicate,
                    valueBinaryExpressionFactory, parentBinaryExpressionFactory);

                thenSourcePredicate?.Invoke(builder);
            }

            return this;
        }

        ICollectionConstantPredicateBuilder<SourceType, ComparisonType> ICollectionConstantPredicateBuilder<SourceType, ComparisonType>.ThenWhereInCollectionConstant<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValue,
            Func<Expression, Expression, BinaryExpression> valueBinaryExpressionFactory,
            Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate,
            Action<ICollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate) =>
            ThenWhereInCollectionConstant(parentBinaryExpressionFactory, getComparisonValue,
                valueBinaryExpressionFactory, sourcePredicate, thenSourcePredicate);

        protected Expression concatenateComparisonExpressions()
        {
            var sourceValueComparisonCount = sourceValueComparisons.Count;
            var concatenatedExpression = sourceValueComparisons[0].SourcePredicate;

            for (int index = 1; index < sourceValueComparisonCount; index++) {
                var sourceValueComparison = sourceValueComparisons[index];
                concatenatedExpression = valueBinaryExpressionFactory(concatenatedExpression, sourceValueComparison.SourcePredicate);
            }

            return concatenatedExpression;
        }

        private void appendConcatenatedExpressionToParent()
        {
            if (parentBinaryExpressionFactory == null) {
                throw new InvalidOperationException("You cannot build the root builder.");
            }

            var concatenatedExpression = concatenateComparisonExpressions();
            parentBuilder.AppendExpression(concatenatedExpression, parentBinaryExpressionFactory);
        }

        void ICollectionConstantPredicateBuilder.AppendConcatenatedExpressionToParent() =>
            appendConcatenatedExpressionToParent();

        public Expression BuildExpression(Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            while (parentBuilder.TryPopBuilder(out var builder)) {
                builder.AppendConcatenatedExpressionToParent();
            }

            var concatenatedExpression = concatenateComparisonExpressions();

            if (concatenatedExpressionFactory != null) {
                concatenatedExpression = concatenatedExpressionFactory(concatenatedExpression);
            }

            return concatenatedExpression;
        }

        public Expression BuildExpression<TargetType>(Action<MemberMappingBuilder<SourceType, TargetType>> outerMemberMappings, Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var concatenatedExpression = BuildExpression(concatenatedExpressionFactory);
            var memberMappings = outerMemberMappings.MemberMappings;

            if (memberMappings == null || memberMappings.Length == 0) {
                return concatenatedExpression;
            }

            var sourceType = typeof(SourceType);

            var relevantMemberMapping = memberMappings
                .Where(x => x.FromMemberPath.HasSourceExpression
                            && x.FromMemberPath.SourceExpression is ParameterExpression parameter
                            && parameter.Type == sourceType)
                .Select(x => {
                    memberMappings.
                });

            var memberPathReplacer = new SourceMemberPathReplacerVisitor(memberMappings);
            concatenatedExpression = memberPathReplacer.Visit(concatenatedExpression);
            return concatenatedExpression;
        }

        public Expression<Func<SourceType, bool>> BuildLambda(Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var concatenatedExpression = BuildExpression(concatenatedExpressionFactory: concatenatedExpressionFactory);
            return Expression.Lambda<Func<SourceType, bool>>(concatenatedExpression, parentBuilder.SourceParameterExpression);
        }

        private readonly struct RootBuilder : IParentCollectionConstantPredicateBuilder
        {
            public readonly Stack<ICollectionConstantPredicateBuilder> BuilderStack { get; }
            public readonly ParameterExpression SourceParameterExpression { get; }

            public readonly bool IsRoot => true;

            public RootBuilder(Stack<ICollectionConstantPredicateBuilder> builderStack, ParameterExpression sourceParameterExpression)
            {
                BuilderStack = builderStack ?? throw new ArgumentNullException(nameof(builderStack));
                SourceParameterExpression = sourceParameterExpression ?? throw new ArgumentNullException(nameof(sourceParameterExpression));
            }

            public void StackBuilder(ICollectionConstantPredicateBuilder builder) =>
                BuilderStack.Push(builder);

            public bool TryPopBuilder([MaybeNullWhen(false)] out ICollectionConstantPredicateBuilder builder)
            {
                if (BuilderStack.Count == 0) {
                    builder = default;
                    return false;
                }

                builder = BuilderStack.Pop();
                return true;
            }

            public void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpression) =>
                throw new NotImplementedException();
        }

        private readonly struct ExpressionAppender
        {
            private readonly int comparisonIndex;
            private readonly IList<SourceValueComparison<ComparisonType>> comparisons;

            public ExpressionAppender(int comparisonIndex, IList<SourceValueComparison<ComparisonType>> comparisons)
            {
                this.comparisonIndex = comparisonIndex;
                this.comparisons = comparisons ?? throw new ArgumentNullException(nameof(comparisons));

                if (comparisonIndex < 0 || comparisonIndex >= comparisons.Count) {
                    throw new ArgumentOutOfRangeException(nameof(comparisonIndex));
                }
            }

            public void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpressionFactory)
            {
                var comparison = comparisons[comparisonIndex];
                var concatenatedExpression = binaryExpressionFactory(comparison.SourcePredicate, expression);
                comparisons[comparisonIndex] = new SourceValueComparison<ComparisonType>(comparison.ComparisonValue, concatenatedExpression);
            }
        }

        private readonly struct ParentBuilder : IParentCollectionConstantPredicateBuilder
        {
            public bool IsRoot => false;
            public ParameterExpression SourceParameterExpression => builder.parentBuilder.SourceParameterExpression;

            private readonly CollectionConstantPredicateBuilder<SourceType, ComparisonType> builder;
            private readonly CollectionConstantPredicateBuilder<SourceType, ComparisonType>.ExpressionAppender expressionAppender;

            public ParentBuilder(CollectionConstantPredicateBuilder<SourceType, ComparisonType> builder, ExpressionAppender expressionAppender)
            {
                this.builder = builder;
                this.expressionAppender = expressionAppender;
            }

            public void StackBuilder(ICollectionConstantPredicateBuilder builder) =>
                this.builder.parentBuilder.StackBuilder(builder);

            public bool TryPopBuilder([MaybeNullWhen(false)] out ICollectionConstantPredicateBuilder builder) =>
                this.builder.parentBuilder.TryPopBuilder(out builder);

            public void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpression) =>
                expressionAppender.AppendExpression(expression, binaryExpression);
        }
    }
}
