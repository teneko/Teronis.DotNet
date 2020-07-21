using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    public class CollectionConstantPredicateBuilder<SourceType, ComparisonType> : IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType>, IChildCollectionConstantPredicateBuilder
    {
        private readonly IParentCollectionConstantPredicateBuilder parentBuilder;
        private List<SourceValueComparison<ComparisonType>> sourceValueComparisons = null!;
        private readonly Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory;
        private readonly Func<Expression, Expression, BinaryExpression>? parentBinaryExpressionFactory;

        /// <summary>
        /// Creates a collection constant predicate builder from a non-null and non-empty collection.
        /// via <paramref name="consecutiveItemBinaryExpressionFactory"/>.
        /// </summary>
        /// <param name="consecutiveItemBinaryExpressionFactory">The binary expression factory combines an item predicate with a previous item predicate.</param>
        /// <param name="comparisonEnumerable">A collection of comparison values with at least one item.</param>
        /// <param name="sourceAndItemPredicate">The expression that represents the predicate.</param>
        /// <exception cref="ArgumentNullException">A parameter is null.</exception>
        /// <exception cref="ArgumentException">The parameter <paramref name="comparisonEnumerable"/> is empty.</exception>
        public CollectionConstantPredicateBuilder(
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            IEnumerable<ComparisonType> comparisonEnumerable,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate)
        {
            if (comparisonEnumerable is null) {
                throw new ArgumentNullException(nameof(comparisonEnumerable));
            }

            if (!comparisonEnumerable.GetEnumerator().MoveNext()) {
                throw new ArgumentException("The comparison list cannot be empty.", nameof(comparisonEnumerable));
            }

            this.consecutiveItemBinaryExpressionFactory = consecutiveItemBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(consecutiveItemBinaryExpressionFactory));
            onConstruction(comparisonEnumerable, sourceAndItemPredicate, out var sourceParameterExpression, null);
            parentBuilder = new RootBuilder(new Stack<IChildCollectionConstantPredicateBuilder>(), sourceParameterExpression);
        }

        private CollectionConstantPredicateBuilder(IParentCollectionConstantPredicateBuilder parentBuilder,
            IEnumerable<ComparisonType> comparisonEnumerable,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate,
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory)
        {
            this.parentBuilder = parentBuilder ?? throw new ArgumentNullException(nameof(parentBuilder));
            comparisonEnumerable = comparisonEnumerable ?? throw new ArgumentNullException(nameof(comparisonEnumerable));
            sourceAndItemPredicate = sourceAndItemPredicate ?? throw new ArgumentNullException(nameof(sourceAndItemPredicate));
            this.consecutiveItemBinaryExpressionFactory = consecutiveItemBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(consecutiveItemBinaryExpressionFactory));
            this.parentBinaryExpressionFactory = parentBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(parentBinaryExpressionFactory));
            onConstruction(comparisonEnumerable, sourceAndItemPredicate, out _, parentBuilder.SourceParameterExpression);
            // We only want to build level one and upward levels.
            parentBuilder.StackBuilder(this);
        }

        private void onConstruction(IEnumerable<ComparisonType> comparisonEnumerable,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonType>> sourceAndItemPredicate,
            out ParameterExpression sourceParameterExpression, ParameterExpression? sourceParameterReplacement)
        {
            sourceParameterExpression = null!;
            sourceValueComparisons = new List<SourceValueComparison<ComparisonType>>();
            var comparisonValueEnumerator = comparisonEnumerable.GetEnumerator();

            while (comparisonValueEnumerator.MoveNext()) {
                var comparisonValue = comparisonValueEnumerator.Current;

                var whereInConstantExpression = SourceExpression.WhereInConstant(comparisonValue, sourceAndItemPredicate,
                    out sourceParameterExpression, sourceParameterReplacement: sourceParameterReplacement);

                var sourceValueComparison = new SourceValueComparison<ComparisonType>(comparisonValue, whereInConstantExpression);
                sourceValueComparisons.Add(sourceValueComparison);
            }
        }

        private CollectionConstantPredicateBuilder<SourceType, ComparisonType> thenDefinePredicatePerItemInCollection<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IEnumerable<ThenComparisonType>?> getComparisonValues,
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonType>> sourcePredicate,
            Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonType>>? thenSourcePredicate = null)
        {
            var sourceValueComparisonsCount = sourceValueComparisons.Count;

            for (int comparisonIndex = 0; comparisonIndex < sourceValueComparisonsCount; comparisonIndex++) {
                var sourceValueComparison = sourceValueComparisons[comparisonIndex];
                var enumerable = getComparisonValues(sourceValueComparison.ComparisonValue);

                if (enumerable is null || !enumerable.GetEnumerator().MoveNext()) {
                    continue;
                }

                var expressionAppender = new ExpressionAppender(comparisonIndex, sourceValueComparisons);
                var parentBuilder = new ParentBuilder(this, expressionAppender);

                var builder = new CollectionConstantPredicateBuilder<SourceType, ThenComparisonType>(parentBuilder, enumerable, sourcePredicate,
                    consecutiveItemBinaryExpressionFactory, parentBinaryExpressionFactory);

                thenSourcePredicate?.Invoke(builder);
            }

            return this;
        }

        /// <summary>
        /// Creates a deferred collection constant predicate builder from <paramref name="getComparisonValues"/> that might return a null or empty collection.
        /// </summary>
        /// <typeparam name="ThenComparisonType">The type of an item of the return value of <paramref name="getComparisonValues"/>.</typeparam>
        /// <param name="getComparisonValues">A collection expression that might return a comparison value list with at least one item.</param>
        /// <returns>A deferred collection constant predicate builder.</returns>
        public DeferredThenCreateBuilder<ThenComparisonType> ThenCreateFromCollection<ThenComparisonType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonType, IReadOnlyCollection<ThenComparisonType>?> getComparisonValues) =>
            new DeferredThenCreateBuilder<ThenComparisonType>(this, parentBinaryExpressionFactory, getComparisonValues);

        IDeferredThenCreateCollectionConstantBuilder<ThenComparisonType> IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonType>.ThenCreateFromCollection<ThenComparisonType>(
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

        void IChildCollectionConstantPredicateBuilder.AppendConcatenatedExpressionToParent() =>
            appendConcatenatedExpressionToParent();

        /// <summary>
        /// Builds the body expression for a possible lambda expression.
        /// </summary>
        /// <param name="concatenatedExpressionFactory">Manipulates the concatenated expression result.</param>
        /// <returns>The body expression for a possible lambda expression.</returns>
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

        /// <summary>
        /// Builds the body expression for a possible lambda expression.
        /// </summary>
        /// <typeparam name="TargetType">The target type you can use for mapping in <paramref name="configureMemberMappings"/>.</typeparam>
        /// <param name="configureMemberMappings">Lets you map source type member accesses to target type member accesses.</param>
        /// <param name="targetParameter">The parameter that served as replacement for the member mappings.</param>
        /// <param name="concatenatedExpressionFactory">Manipulates the concatenated expression result.</param>
        /// <returns>The body expression for a possible lambda expression.</returns>
        internal Expression BuildBodyExpression<TargetType>(Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings,
            out ParameterExpression targetParameter, Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var concatenatedExpression = BuildBodyExpression(concatenatedExpressionFactory);

            concatenatedExpression = SourceExpression.ReplaceParameter(concatenatedExpression, parentBuilder.SourceParameterExpression,
                configureMemberMappings, out targetParameter);

            return concatenatedExpression;
        }

        /// <summary>
        /// Builds the body expression for a possible lambda expression.
        /// </summary>
        /// <typeparam name="TargetType">The target type you can use for mapping in <paramref name="configureMemberMappings"/>.</typeparam>
        /// <param name="configureMemberMappings">Lets you map source type member accesses to target type member accesses.</param>
        /// <param name="concatenatedExpressionFactory">Manipulates the concatenated expression result.</param>
        /// <returns>The body expression for a possible lambda expression.</returns>
        public Expression BuildBodyExpression<TargetType>(Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings,
            Func<Expression, Expression>? concatenatedExpressionFactory = null) =>
            BuildBodyExpression(configureMemberMappings, out _, concatenatedExpressionFactory);

        private Expression<Func<ParameterType, bool>> buildLambdaExpression<ParameterType>(Expression body, ParameterExpression parameter) =>
            Expression.Lambda<Func<ParameterType, bool>>(body, parameter);

        /// <summary>
        /// Builds the lambda expression.
        /// </summary>
        /// <param name="concatenatedExpressionFactory">Manipulates the concatenated expression result.</param>
        /// <returns>The lambda expression.</returns>
        public Expression<Func<SourceType, bool>> BuildLambdaExpression(Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var concatenatedExpression = BuildBodyExpression(concatenatedExpressionFactory: concatenatedExpressionFactory);
            return buildLambdaExpression<SourceType>(concatenatedExpression, parentBuilder.SourceParameterExpression);
        }

        /// <summary>
        /// Builds the lambda expression.
        /// </summary>
        /// <typeparam name="TargetType">The target type you can use for mapping in <paramref name="configureMemberMappings"/>.</typeparam>
        /// <param name="configureMemberMappings">Lets you map source type member accesses to target type member accesses.</param>
        /// <param name="concatenatedExpressionFactory">Manipulates the concatenated expression result.</param>
        /// <returns>The lambda expression.</returns>
        public Expression<Func<TargetType, bool>> BuildLambdaExpression<TargetType>(Action<IMappableTypedSourceTargetMembers<SourceType, TargetType>> configureMemberMappings,
            Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var bodyExpression = BuildBodyExpression(configureMemberMappings, out var targetParameter, concatenatedExpressionFactory);
            return buildLambdaExpression<TargetType>(bodyExpression, parentBuilder.SourceParameterExpression);
        }

        private readonly struct RootBuilder : IParentCollectionConstantPredicateBuilder
        {
            public readonly Stack<IChildCollectionConstantPredicateBuilder> BuilderStack { get; }
            public readonly ParameterExpression SourceParameterExpression { get; }

            public readonly bool IsRoot => true;

            public RootBuilder(Stack<IChildCollectionConstantPredicateBuilder> builderStack, ParameterExpression sourceParameterExpression)
            {
                BuilderStack = builderStack ?? throw new ArgumentNullException(nameof(builderStack));
                SourceParameterExpression = sourceParameterExpression ?? throw new ArgumentNullException(nameof(sourceParameterExpression));
            }

            public void StackBuilder(IChildCollectionConstantPredicateBuilder builder) =>
                BuilderStack.Push(builder);

            public bool TryPopBuilder([MaybeNullWhen(false)] out IChildCollectionConstantPredicateBuilder builder)
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

            public void StackBuilder(IChildCollectionConstantPredicateBuilder builder) =>
                this.builder.parentBuilder.StackBuilder(builder);

            public bool TryPopBuilder([MaybeNullWhen(false)] out IChildCollectionConstantPredicateBuilder builder) =>
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
