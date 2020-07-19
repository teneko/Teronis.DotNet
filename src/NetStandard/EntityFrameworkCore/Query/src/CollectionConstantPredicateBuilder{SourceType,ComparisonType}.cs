using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public class CollectionConstantPredicateBuilder<SourceType, ComparisonType> : IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType>, ICollectionConstantPredicateBuilder
    {
        private readonly IParentCollectionConstantPredicateBuilder parentBuilder;
        private List<SourceValueComparison<ComparisonType>> sourceValueComparisons = null!;
        private readonly Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory;
        private readonly Func<Expression, Expression, BinaryExpression>? parentBinaryExpressionFactory;

        public CollectionConstantPredicateBuilder(
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            IReadOnlyCollection<ComparisonType> comparisonList,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate)
        {
            if (comparisonList is null || comparisonList.Count == 0) {
                throw new ArgumentNullException(nameof(comparisonList));
            }

            this.consecutiveItemBinaryExpressionFactory = consecutiveItemBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(consecutiveItemBinaryExpressionFactory));
            onConstruction(comparisonList, sourceAndItemPredicate, out var sourceParameterExpression, null);
            parentBuilder = new RootBuilder(new Stack<ICollectionConstantPredicateBuilder>(), sourceParameterExpression);
        }

        private CollectionConstantPredicateBuilder(IParentCollectionConstantPredicateBuilder parentBuilder,
            IReadOnlyCollection<ComparisonType> comparisonList,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate,
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory)
        {
            this.parentBuilder = parentBuilder ?? throw new ArgumentNullException(nameof(parentBuilder));
            comparisonList = comparisonList ?? throw new ArgumentNullException(nameof(comparisonList));
            sourceAndItemPredicate = sourceAndItemPredicate ?? throw new ArgumentNullException(nameof(sourceAndItemPredicate));
            this.consecutiveItemBinaryExpressionFactory = consecutiveItemBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(consecutiveItemBinaryExpressionFactory));
            this.parentBinaryExpressionFactory = parentBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(parentBinaryExpressionFactory));
            onConstruction(comparisonList, sourceAndItemPredicate, out _, parentBuilder.SourceParameterExpression);
            // We only want to build level one and upward levels.
            parentBuilder.StackBuilder(this);
        }

        private void onConstruction(IReadOnlyCollection<ComparisonType> comparisonList,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate,
            out ParameterExpression sourceParameterExpression, ParameterExpression? sourceParameterReplacement)
        {
            sourceParameterExpression = null!;
            sourceValueComparisons = new List<SourceValueComparison<ComparisonType>>();
            var comparisonValueEnumerator = comparisonList.GetEnumerator();

            while (comparisonValueEnumerator.MoveNext()) {
                var comparisonValue = comparisonValueEnumerator.Current;

                var whereInConstantExpression = SourceExpression.WhereInConstant(comparisonValue, sourceAndItemPredicate,
                    out sourceParameterExpression, sourceParameterReplacement: sourceParameterReplacement);

                var sourceValueComparison = new SourceValueComparison<ComparisonType>(comparisonValue, whereInConstantExpression);
                sourceValueComparisons.Add(sourceValueComparison);
            }
        }

        public DeferredThenCreateBuilder<ThenComparisonType> ThenCreateFromCollection<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues) =>
            new DeferredThenCreateBuilder<ThenComparisonType>(this, parentBinaryExpressionFactory, getComparisonValues);

        private CollectionConstantPredicateBuilder<SourceType, ComparisonType> thenDefinePredicatePerItemInCollection<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues,
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate,
            Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate = null)
        {
            var sourceValueComparisonsCount = sourceValueComparisons.Count;

            for (int comparisonIndex = 0; comparisonIndex < sourceValueComparisonsCount; comparisonIndex++) {
                var sourceValueComparison = sourceValueComparisons[comparisonIndex];
                var list = getComparisonValues(sourceValueComparison.ComparisonValue);

                if (list is null || list.Count == 0) {
                    continue;
                }

                var expressionAppender = new ExpressionAppender(comparisonIndex, sourceValueComparisons);
                var parentBuilder = new ParentBuilder(this, expressionAppender);

                var builder = new CollectionConstantPredicateBuilder<SourceType, ThenComparisonType>(parentBuilder, list, sourcePredicate,
                    consecutiveItemBinaryExpressionFactory, parentBinaryExpressionFactory);

                thenSourcePredicate?.Invoke(builder);
            }

            return this;
        }

        IDeferredThenCreateCollectionConstantBuilder<ThenComparisonType> IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType>.ThenInCollection<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues) =>
            ThenCreateFromCollection(parentBinaryExpressionFactory, getComparisonValues);

        protected Expression concatenateComparisonExpressions()
        {
            var sourceValueComparisonCount = sourceValueComparisons.Count;
            var concatenatedExpression = sourceValueComparisons[0].SourcePredicate;

            for (int index = 1; index < sourceValueComparisonCount; index++) {
                var sourceValueComparison = sourceValueComparisons[index];
                concatenatedExpression = consecutiveItemBinaryExpressionFactory(concatenatedExpression, sourceValueComparison.SourcePredicate);
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

        public Expression BuildBodyExpression(Func<Expression, Expression>? concatenatedExpressionFactory = null)
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

        internal Expression BuildBodyExpression<TargetType>(Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings,
            out ParameterExpression targetParameter, Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var concatenatedExpression = BuildBodyExpression(concatenatedExpressionFactory);
            targetParameter = Expression.Parameter(typeof(TargetType), "targetAsSource");
            var memberMappingBuilder = new NodeReplacingMemberMappingBuilder<SourceType, TargetType>(parentBuilder.SourceParameterExpression, targetParameter);
            configureMemberMappings(memberMappingBuilder);
            var memberMappings = memberMappingBuilder.GetMappings().ToList();

            if (memberMappings == null || memberMappings.Count == 0) {
                return concatenatedExpression;
            }

            var memberPathReplacer = new SourceMemberPathReplacerVisitor(memberMappings);
            concatenatedExpression = memberPathReplacer.Visit(concatenatedExpression);
            return concatenatedExpression;
        }

        public Expression BuildBodyExpression<TargetType>(Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings,
            Func<Expression, Expression>? concatenatedExpressionFactory = null) =>
            BuildBodyExpression(configureMemberMappings, out _, concatenatedExpressionFactory);

        private Expression<Func<ParameterType, bool>> buildLambdaExpression<ParameterType>(Expression body, ParameterExpression parameter) =>
            Expression.Lambda<Func<ParameterType, bool>>(body, parameter);

        public Expression<Func<SourceType, bool>> BuildLambdaExpression(Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var concatenatedExpression = BuildBodyExpression(concatenatedExpressionFactory: concatenatedExpressionFactory);
            return buildLambdaExpression<SourceType>(concatenatedExpression, parentBuilder.SourceParameterExpression);
        }

        public Expression<Func<TargetType, bool>> BuildLambdaExpression<TargetType>(Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings,
            Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var bodyExpression = BuildBodyExpression(configureMemberMappings, out var targetParameter, concatenatedExpressionFactory);
            return buildLambdaExpression<TargetType>(bodyExpression, parentBuilder.SourceParameterExpression);
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

        public interface IDeferredThenCreateCollectionConstantBuilder<ThenComparisonType>
        {
            IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType> DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory, 
                Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate, 
                Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate = null);
        }

        public readonly struct DeferredThenCreateBuilder<ThenComparisonType> : IDeferredThenCreateCollectionConstantBuilder<ThenComparisonType>
        {
            private readonly CollectionConstantPredicateBuilder<SourceType, ComparisonType> currentBuilder;
            private readonly Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory;
            private readonly Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues;

            public DeferredThenCreateBuilder(CollectionConstantPredicateBuilder<SourceType, ComparisonType> currentBuilder,
                Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
                Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues)
            {
                this.currentBuilder = currentBuilder;
                this.parentBinaryExpressionFactory = parentBinaryExpressionFactory;
                this.getComparisonValues = getComparisonValues;
            }

            public CollectionConstantPredicateBuilder<SourceType, ComparisonType> DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate,
                Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate = null) =>
                currentBuilder.thenDefinePredicatePerItemInCollection(parentBinaryExpressionFactory, getComparisonValues,
                    consecutiveItemBinaryExpressionFactory, sourcePredicate, thenSourcePredicate);

            IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType> CollectionConstantPredicateBuilder<SourceType, ComparisonType>.IDeferredThenCreateCollectionConstantBuilder<ThenComparisonType>.DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate,
                Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate) =>
                DefinePredicatePerItem(consecutiveItemBinaryExpressionFactory, sourcePredicate, thenSourcePredicate);
        }
    }
}
