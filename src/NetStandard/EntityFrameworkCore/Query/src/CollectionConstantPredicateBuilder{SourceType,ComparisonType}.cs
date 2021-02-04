using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Teronis.Linq.Expressions;

namespace Teronis.EntityFrameworkCore.Query
{
    /// <summary>
    /// The collection constant predicate builder produces one predicate that includes
    /// the comparison between the source of type <typeparamref name="SourceType"/> and
    /// each collection item of type <typeparamref name="ComparisonItemType"/>. Each
    /// collection item can be itself a source of type <typeparamref name="ComparisonItemType"/>
    /// and therefore be compared to collection item of a collection that is member of
    /// <typeparamref name="ComparisonItemType"/> and so on.
    /// <br/>
    /// <br/>(e.g. src1.A == cmpItem1.Z
    /// </summary>
    /// <typeparam name="SourceType"></typeparam>
    /// <typeparam name="ComparisonItemType"></typeparam>
    public class CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> : IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonItemType>, IChildCollectionConstantPredicateBuilder
    {
        private readonly IParentCollectionConstantPredicateBuilder parentBuilder;
        /// <summary>
        /// Initialized and filled on construction.
        /// </summary>
        private List<ComparisonItemAndPredicate> cachedComparisonItemAndPredicateList = null!;
        private readonly Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory;
        private readonly Func<Expression, Expression, BinaryExpression>? parentBinaryExpressionFactory;

        /// <summary>
        /// Creates a collection constant predicate builder from a non-null and non-empty collection. Otherwise
        /// <see cref="ArgumentNullException"/> will be thrown. When starting with a non-null and non-empty colection
        /// you are able to use <see cref="ThenCreateFromCollection{ThenComparisonItemType}(Func{Expression, Expression, BinaryExpression}, Func{ComparisonItemType, IEnumerable{ThenComparisonItemType}?}, ComparisonItemsBehaviourFlags)"/>
        /// and this collection can then be null or empy.
        /// </summary>
        /// <param name="consecutiveItemBinaryExpressionFactory">
        /// The binary expression factory that combines an item predicate, an item predicate that is arisen from 
        /// <paramref name="comparisonItems"/> and that encloses every item predicate of item
        /// predicates of child collection and their collections, with another item predicate arisen from
        /// <paramref name="comparisonItems"/> that encloses every item predicate of item
        /// predicates of child collection and their collections.
        /// </param>
        /// <param name="comparisonItems">
        /// A collection of comparison values with at least one item. Otherwise an <see cref="ArgumentNullException"/>
        /// is thrown.
        /// </param>
        /// <param name="sourceAndComparisonItemPredicate">The expression that represents the predicate.</param>
        /// <exception cref="ArgumentNullException">A parameter <paramref name="comparisonItems"/> is null.</exception>
        /// <exception cref="ArgumentException">The parameter <paramref name="comparisonItems"/> is empty.</exception>
        public CollectionConstantPredicateBuilder(
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            IEnumerable<ComparisonItemType> comparisonItems,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonItemType>> sourceAndComparisonItemPredicate)
        {
            comparisonItems = comparisonItems ??
                throw new ArgumentNullException(nameof(comparisonItems));

            var comparisonItemEnumerator = comparisonItems.GetEnumerator();

            if (!comparisonItemEnumerator.MoveNext()) {
                throw new ArgumentException("The comparison item list cannot be empty.", nameof(comparisonItems));
            }

            this.consecutiveItemBinaryExpressionFactory = consecutiveItemBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(consecutiveItemBinaryExpressionFactory));
            onConstruction(comparisonItemEnumerator, true, sourceAndComparisonItemPredicate, out var sourceParameterExpression, null);
            parentBuilder = new RootBuilder(new Stack<IChildCollectionConstantPredicateBuilder>(), sourceParameterExpression);
        }

        /// <summary>
        /// You have to call
        /// <see cref="onConstruction(IEnumerator{ComparisonItemType}, bool, Expression{SourceInConstantPredicateDelegate{SourceType, ComparisonItemType}}, out ParameterExpression, ParameterExpression?)"/>
        /// manually.
        /// </summary>
        /// <param name="parentBuilder"></param>
        /// <param name="consecutiveItemBinaryExpressionFactory"></param>
        /// <param name="parentBinaryExpressionFactory"></param>
        private CollectionConstantPredicateBuilder(IParentCollectionConstantPredicateBuilder parentBuilder,
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory)
        {
            this.parentBuilder = parentBuilder ?? throw new ArgumentNullException(nameof(parentBuilder));
            this.consecutiveItemBinaryExpressionFactory = consecutiveItemBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(consecutiveItemBinaryExpressionFactory));
            this.parentBinaryExpressionFactory = parentBinaryExpressionFactory ?? throw new ArgumentNullException(nameof(parentBinaryExpressionFactory));
        }

        /// <summary>
        /// Gets called on construction of the very first collection and manually on every further collection.
        /// </summary>
        /// <param name="comparisonItemEnumerator"></param>
        /// <param name="comparisonItemEnumeratorMovedToFirst"></param>
        /// <param name="sourceAndComparisonItemPredicate"></param>
        /// <param name="sourceParameterExpression"></param>
        /// <param name="sourceParameterReplacement"></param>
        private void onConstruction(
            IEnumerator<ComparisonItemType> comparisonItemEnumerator, 
            bool comparisonItemEnumeratorMovedToFirst,
            Expression<SourceInConstantPredicateDelegate<SourceType, ComparisonItemType>> sourceAndComparisonItemPredicate,
            out ParameterExpression sourceParameterExpression, ParameterExpression? sourceParameterReplacement)
        {
            cachedComparisonItemAndPredicateList = new List<ComparisonItemAndPredicate>();
            sourceParameterExpression = null!;

            Dictionary<int, ParameterExpression>? positionalParameterReplacements;

            if (sourceParameterReplacement is null) {
                positionalParameterReplacements = null;
            } else {
                positionalParameterReplacements = new Dictionary<int, ParameterExpression>() { { 0, sourceParameterReplacement } };
            }

            while (comparisonItemEnumeratorMovedToFirst || comparisonItemEnumerator.MoveNext()) {
                comparisonItemEnumeratorMovedToFirst = false;
                var comparisonItem = comparisonItemEnumerator.Current;

                var whereInConstantExpression = SourceTargetExpressionTools.ReplaceParameterByConstantLambdaBody(
                    sourceAndComparisonItemPredicate,
                    positionalParameterReplacements,
                    new Dictionary<int, object?>() { { 1, comparisonItem } },
                    out var positionalParameters);

                sourceParameterExpression = positionalParameters[0];

                //var whereInConstantExpression = SourceTargetExpressionTools.ReplaceParameterByConstantLambdaBody(comparisonItem, sourceAndComparisonItemPredicate,
                //    out sourceParameterExpression, sourceParameterReplacement: sourceParameterReplacement);

                var comparisonItemAndPredicate = new ComparisonItemAndPredicate(comparisonItem, whereInConstantExpression);
                cachedComparisonItemAndPredicateList.Add(comparisonItemAndPredicate);
            }
        }

        /// <summary>
        /// Used in nested collections.
        /// </summary>
        /// <typeparam name="ThenComparisonItemType"></typeparam>
        /// <param name="parentBinaryExpressionFactory"></param>
        /// <param name="comparisonItemsFactory"></param>
        /// <param name="comparisonItemsBehaviourFlags"></param>
        /// <param name="consecutiveItemBinaryExpressionFactory"></param>
        /// <param name="sourceAndComparisonItemPredicate"></param>
        /// <param name="thenSourcePredicate"></param>
        /// <returns></returns>
        private CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> thenDefinePredicatePerItemInCollection<ThenComparisonItemType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonItemType, IEnumerable<ThenComparisonItemType>?> comparisonItemsFactory,
            ComparisonItemsBehaviourFlags comparisonItemsBehaviourFlags,
            Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
            Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonItemType>> sourceAndComparisonItemPredicate,
            Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonItemType>>? thenSourcePredicate = null)
        {
            var sourceValueComparisonsCount = cachedComparisonItemAndPredicateList.Count;

            for (int comparisonIndex = 0; comparisonIndex < sourceValueComparisonsCount; comparisonIndex++) {
                var sourceAndComparisonItem = cachedComparisonItemAndPredicateList[comparisonIndex];
                var comparisonItems = comparisonItemsFactory(sourceAndComparisonItem.ComparisonItem);
                var comparisonItemsEnumerator = comparisonItems?.GetEnumerator();
                var comparisonItemsEnumeratorIsNull = comparisonItemsEnumerator is null;
                var comparisonItemsEnumeratorIsEmpty = !comparisonItemsEnumeratorIsNull && !comparisonItemsEnumerator!.MoveNext();

                var evaluateToFalse = (comparisonItemsEnumeratorIsNull && comparisonItemsBehaviourFlags.HasFlag(ComparisonItemsBehaviourFlags.NullLeadsToFalse))
                    || (comparisonItemsEnumeratorIsEmpty && comparisonItemsBehaviourFlags.HasFlag(ComparisonItemsBehaviourFlags.EmptyLeadsToFalse));

                if ((comparisonItemsEnumeratorIsNull || comparisonItemsEnumeratorIsEmpty)
                    && !evaluateToFalse) {
                    continue;
                }

                var expressionAppender = new AppendablePredicates(comparisonIndex, cachedComparisonItemAndPredicateList);
                var parentBuilder = new ParentBuilder(this, expressionAppender);

                IChildCollectionConstantPredicateBuilder childBuilder;
                IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonItemType>? thenInCollectionBuilder;

                if (evaluateToFalse) {
                    childBuilder = new FalseEvaluatingChildBuilder(parentBuilder);
                    thenInCollectionBuilder = null;
                } else {
                    var builder = new CollectionConstantPredicateBuilder<SourceType, ThenComparisonItemType>(parentBuilder,
                        consecutiveItemBinaryExpressionFactory, parentBinaryExpressionFactory);

                    builder.onConstruction(comparisonItemsEnumerator!,
                        comparisonItemEnumeratorMovedToFirst: true, 
                        sourceAndComparisonItemPredicate,
                        sourceParameterExpression: out _, 
                        parentBuilder.SourceParameterExpression);

                    childBuilder = builder;
                    thenInCollectionBuilder = builder;
                }

                // 
                parentBuilder.StackBuilder(childBuilder);

                if (!(thenInCollectionBuilder is null)) {
                    thenSourcePredicate?.Invoke(thenInCollectionBuilder);
                }
            }

            return this;
        }

        /// <summary>
        /// Creates a deferred collection constant predicate builder from <paramref name="comparisonItemsFactory"/> that might return a null or 
        /// empty collection.
        /// </summary>
        /// <typeparam name="ThenComparisonItemType">The type of an item of the return value of <paramref name="comparisonItemsFactory"/>.</typeparam>
        /// <param name="comparisonItemsFactory">A collection expression that might return a comparison value list with at least one item.</param>
        /// <returns>A deferred collection constant predicate builder.</returns>
        public DeferredThenCreateBuilder<ThenComparisonItemType> ThenCreateFromCollection<ThenComparisonItemType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonItemType, IEnumerable<ThenComparisonItemType>?> comparisonItemsFactory,
            ComparisonItemsBehaviourFlags comparisonItemsBehaviourFlags = ComparisonItemsBehaviourFlags.NullOrEmptyLeadsToSkip) =>
            new DeferredThenCreateBuilder<ThenComparisonItemType>(this, parentBinaryExpressionFactory, comparisonItemsFactory, comparisonItemsBehaviourFlags);

        IDeferredThenCreateCollectionConstantBuilder<ThenComparisonItemType> IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonItemType>.ThenCreateFromCollection<ThenComparisonItemType>(
            Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
            Func<ComparisonItemType, IEnumerable<ThenComparisonItemType>?> comparisonItemsFactory,
            ComparisonItemsBehaviourFlags comparisonItemsBehaviourFlags) =>
            ThenCreateFromCollection(parentBinaryExpressionFactory, comparisonItemsFactory, comparisonItemsBehaviourFlags);

        protected Expression concatenateComparisonExpressions()
        {
            var sourceValueComparisonCount = cachedComparisonItemAndPredicateList.Count;
            var concatenatedExpression = cachedComparisonItemAndPredicateList[0].SourceAndItemPredicate;

            for (int index = 1; index < sourceValueComparisonCount; index++) {
                var sourceValueComparison = cachedComparisonItemAndPredicateList[index];
                concatenatedExpression = consecutiveItemBinaryExpressionFactory(concatenatedExpression, sourceValueComparison.SourceAndItemPredicate);
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
        internal Expression BuildBodyExpression<TargetType>(Action<ICollectionConstantPredicateBuilderExpressionMapper<SourceType, TargetType>> configureMemberMappings,
            out ParameterExpression targetParameter, Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var concatenatedExpression = BuildBodyExpression(concatenatedExpressionFactory);
            targetParameter = Expression.Parameter(typeof(TargetType), "sourceAsTarget");

            var expressionMapper = new CollectionConstantPredicateBuilderExpressionMapper<TargetType>(parentBuilder.SourceParameterExpression,
                targetParameter);

            configureMemberMappings(expressionMapper);
            var expressionMappings = expressionMapper.GetMappings();
            var expressionReplacer = new EqualityComparingExpressionReplacerVisitor(expressionMappings);
            concatenatedExpression = expressionReplacer.Visit(concatenatedExpression);
            return concatenatedExpression;
        }

        /// <summary>
        /// Builds the body expression for a possible lambda expression.
        /// </summary>
        /// <typeparam name="TargetType">The target type you can use for mapping in <paramref name="configureMemberMappings"/>.</typeparam>
        /// <param name="configureMemberMappings">Lets you map source type member accesses to target type member accesses.</param>
        /// <param name="concatenatedExpressionFactory">Manipulates the concatenated expression result.</param>
        /// <returns>The body expression for a possible lambda expression.</returns>
        public Expression BuildBodyExpression<TargetType>(Action<ICollectionConstantPredicateBuilderExpressionMapper<SourceType, TargetType>> configureMemberMappings,
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
        public Expression<Func<TargetType, bool>> BuildLambdaExpression<TargetType>(
            Action<ICollectionConstantPredicateBuilderExpressionMapper<SourceType, TargetType>> configureMemberMappings,
            Func<Expression, Expression>? concatenatedExpressionFactory = null)
        {
            var bodyExpression = BuildBodyExpression(configureMemberMappings, out var targetParameter, concatenatedExpressionFactory);
            return buildLambdaExpression<TargetType>(bodyExpression, targetParameter);
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

        private readonly struct ComparisonItemAndPredicate
        {
            public readonly ComparisonItemType ComparisonItem;
            public readonly Expression SourceAndItemPredicate;

            public ComparisonItemAndPredicate(ComparisonItemType comparisonValue, Expression sourceAndComparisonItemPredicate)
            {
                ComparisonItem = comparisonValue;
                SourceAndItemPredicate = sourceAndComparisonItemPredicate;
            }
        }

        /// <summary>
        /// The struct that represents the caches for all item predicates
        /// </summary>
        private readonly struct AppendablePredicates
        {
            private readonly int comparisonItemAndPredicateIndex;
            private readonly IList<ComparisonItemAndPredicate> comparisonItemAndPredicateList;

            public AppendablePredicates(int comparisonItemAndPredicateIndex, IList<ComparisonItemAndPredicate> comparisonItemAndPredicateList)
            {
                this.comparisonItemAndPredicateIndex = comparisonItemAndPredicateIndex;
                this.comparisonItemAndPredicateList = comparisonItemAndPredicateList ?? throw new ArgumentNullException(nameof(comparisonItemAndPredicateList));

                if (comparisonItemAndPredicateIndex < 0 || comparisonItemAndPredicateIndex >= comparisonItemAndPredicateList.Count) {
                    throw new ArgumentOutOfRangeException(nameof(comparisonItemAndPredicateIndex));
                }
            }

            public void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpressionFactory)
            {
                var comparison = comparisonItemAndPredicateList[comparisonItemAndPredicateIndex];
                var concatenatedExpression = binaryExpressionFactory(comparison.SourceAndItemPredicate, expression);
                comparisonItemAndPredicateList[comparisonItemAndPredicateIndex] = new ComparisonItemAndPredicate(comparison.ComparisonItem, concatenatedExpression);
            }
        }

        private readonly struct ParentBuilder : IParentCollectionConstantPredicateBuilder
        {
            public bool IsRoot => false;
            public ParameterExpression SourceParameterExpression => builder.parentBuilder.SourceParameterExpression;

            private readonly CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> builder;
            private readonly CollectionConstantPredicateBuilder<SourceType, ComparisonItemType>.AppendablePredicates expressionAppender;

            public ParentBuilder(CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> builder, AppendablePredicates expressionAppender)
            {
                this.builder = builder;
                this.expressionAppender = expressionAppender;
            }

            /// <summary>
            /// Puts <paramref name="builder"/> on top of parent builder.
            /// </summary>
            /// <param name="builder"></param>
            public void StackBuilder(IChildCollectionConstantPredicateBuilder builder) =>
                this.builder.parentBuilder.StackBuilder(builder);

            public bool TryPopBuilder([MaybeNullWhen(false)] out IChildCollectionConstantPredicateBuilder builder) =>
                this.builder.parentBuilder.TryPopBuilder(out builder);

            public void AppendExpression(Expression expression, Func<Expression, Expression, BinaryExpression> binaryExpression) =>
                expressionAppender.AppendExpression(expression, binaryExpression);
        }

        private readonly struct FalseEvaluatingChildBuilder : IChildCollectionConstantPredicateBuilder
        {
            private readonly ParentBuilder? parentBuilder;

            public FalseEvaluatingChildBuilder(ParentBuilder parentBuilder) =>
                this.parentBuilder = parentBuilder;

            public void AppendConcatenatedExpressionToParent()
            {
                var parentBuilder = this.parentBuilder ?? throw new InvalidOperationException("Parent builder has not been initialized.");
                parentBuilder.AppendExpression(Expression.Constant(false), Expression.AndAlso);
            }
        }

        public interface IDeferredThenCreateCollectionConstantBuilder<ThenComparisonItemType>
        {
            /// <summary>
            /// Defines predicate for each item in previous selected collection.
            /// </summary>
            /// <param name="consecutiveItemBinaryExpressionFactory"></param>
            /// <param name="sourceAndComparisonItemPredicate"></param>
            /// <param name="thenSourcePredicate"></param>
            /// <returns></returns>
            IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonItemType> DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonItemType>> sourceAndComparisonItemPredicate,
                Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonItemType>>? thenSourcePredicate = null);
        }

        public readonly struct DeferredThenCreateBuilder<ThenComparisonItemType> : IDeferredThenCreateCollectionConstantBuilder<ThenComparisonItemType>
        {
            private readonly CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> currentBuilder;
            private readonly Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory;
            private readonly Func<ComparisonItemType, IEnumerable<ThenComparisonItemType>?> comparisonItemsFactory;
            private readonly ComparisonItemsBehaviourFlags comparisonItemsBehaviourFlags;

            public DeferredThenCreateBuilder(CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> currentBuilder,
                Func<Expression, Expression, BinaryExpression> parentBinaryExpressionFactory,
                Func<ComparisonItemType, IEnumerable<ThenComparisonItemType>?> comparisonItemsFactory,
                ComparisonItemsBehaviourFlags comparisonItemsBehaviourFlags)
            {
                this.currentBuilder = currentBuilder;
                this.parentBinaryExpressionFactory = parentBinaryExpressionFactory;
                this.comparisonItemsFactory = comparisonItemsFactory;
                this.comparisonItemsBehaviourFlags = comparisonItemsBehaviourFlags;
            }

            public CollectionConstantPredicateBuilder<SourceType, ComparisonItemType> ThenDefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonItemType>> sourceAndComparisonItemPredicate,
                Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonItemType>>? thenSourcePredicate = null) =>
                currentBuilder.thenDefinePredicatePerItemInCollection(parentBinaryExpressionFactory, comparisonItemsFactory,
                    comparisonItemsBehaviourFlags, consecutiveItemBinaryExpressionFactory, sourceAndComparisonItemPredicate, thenSourcePredicate);

            IThenInCollectionConstantPredicateBuilder<SourceType, ComparisonItemType> CollectionConstantPredicateBuilder<SourceType, ComparisonItemType>.IDeferredThenCreateCollectionConstantBuilder<ThenComparisonItemType>.DefinePredicatePerItem(
                Func<Expression, Expression, BinaryExpression> consecutiveItemBinaryExpressionFactory,
                Expression<SourceInConstantPredicateDelegate<SourceType, ThenComparisonItemType>> sourceAndComparisonItemPredicate,
                Action<IThenInCollectionConstantPredicateBuilder<SourceType, ThenComparisonItemType>>? thenSourcePredicate) =>
                ThenDefinePredicatePerItem(consecutiveItemBinaryExpressionFactory, sourceAndComparisonItemPredicate, thenSourcePredicate);
        }

        private class CollectionConstantPredicateBuilderExpressionMapper<TargetType> : ParameterReplacingExpressionMapper<SourceType, TargetType>,
            ICollectionConstantPredicateBuilderExpressionMapper<SourceType, TargetType>
        {
            public CollectionConstantPredicateBuilderExpressionMapper(ParameterExpression sourceParameterReplacement, ParameterExpression targetParameterReplacement)
                : base(sourceParameterReplacement, targetParameterReplacement) { }
        }
    }
}
