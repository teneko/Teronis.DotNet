using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Algorithms
{
    public static class SortedCollectionModifications
    {
        /// <summary>
        /// Yields the collection modifications for <paramref name="leftItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="presumedOrder"/>.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="LeftItemType">The typ of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="presumedOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <param name="actions">The actions that regulates how <paramref name="leftItems"/> and <paramref name="rightItems"/> are synchronized.</param>
        /// <returns>The collection modifications for <paramref name="leftItems"/></returns>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            SortedCollectionModificationsOrder presumedOrder,
            IComparer<ComparablePartType>? comparer = null,
            CollectionModificationsActions actions = CollectionModificationsActions.All)
        {
            comparer = comparer ?? Comparer<ComparablePartType>.Default;

            var canInsert = actions.HasFlag(CollectionModificationsActions.Insert);
            var canRemove = actions.HasFlag(CollectionModificationsActions.Remove);
            var canReplace = actions.HasFlag(CollectionModificationsActions.Replace);

            var leftItemsEnumerator = leftItems.GetEnumerator();
            bool leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();

            var rightItemsEnumerator = rightItems.GetEnumerator();
            bool rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();

            int leftIndex = 0;

            while (leftItemsEnumeratorIsFunctional || rightItemsEnumeratorIsFunctional) {
                if (!rightItemsEnumeratorIsFunctional) {
                    if (canRemove) {
                        var leftItem = leftItemsEnumerator.Current;

                        yield return CollectionModification<LeftItemType, RightItemType>.CreateOld(
                                NotifyCollectionChangedAction.Remove,
                                leftItem,
                                leftIndex);
                    } else {
                        leftIndex++;
                    }

                    leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                } else if (!leftItemsEnumeratorIsFunctional) {
                    if (canInsert) {
                        var rightItem = rightItemsEnumerator.Current;

                        yield return CollectionModification<LeftItemType, RightItemType>.CreateNew(
                            NotifyCollectionChangedAction.Add,
                            rightItem,
                            leftIndex);

                        leftIndex++;
                    }

                    rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                } else {
                    var leftItem = leftItemsEnumerator.Current;
                    var rightItem = rightItemsEnumerator.Current;

                    var comparablePartOfLeftItem = getComparablePartOfLeftItem(leftItem);
                    var comparablePartOfRightItem = getComparablePartOfRightItem(rightItem);

                    var comparablePartComparison = presumedOrder == SortedCollectionModificationsOrder.Ascending
                        ? comparer.Compare(comparablePartOfLeftItem, comparablePartOfRightItem)
                        : comparer.Compare(comparablePartOfRightItem, comparablePartOfLeftItem);

                    if (comparablePartComparison < 0) {
                        if (canRemove) {
                            yield return CollectionModification<LeftItemType, RightItemType>.CreateOld(
                                NotifyCollectionChangedAction.Remove,
                                leftItem,
                                leftIndex);
                        } else {
                            leftIndex++;
                        }

                        leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                    } else if (comparablePartComparison > 0) {
                        if (canInsert) {
                            yield return CollectionModification<LeftItemType, RightItemType>.CreateNew(
                            NotifyCollectionChangedAction.Add,
                            rightItem,
                            leftIndex);

                            leftIndex++;
                        }

                        rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                    } else {
                        if (canReplace) {
                            yield return new CollectionModification<LeftItemType, RightItemType>(
                                NotifyCollectionChangedAction.Replace,
                                leftItem,
                                leftIndex,
                                rightItem,
                                leftIndex);
                        }

                        leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                        rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                        leftIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Yields the collection modifications for <paramref name="leftItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="presumedOrder"/>.
        /// descending order.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="ItemType">The typ of left and right items.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="presumedOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <param name="actions">The actions that regulates how <paramref name="leftItems"/> and <paramref name="rightItems"/> are synchronized.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            SortedCollectionModificationsOrder presumedOrder,
            IComparer<ItemType>? comparer = null,
            CollectionModificationsActions actions = CollectionModificationsActions.All) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                presumedOrder,
                comparer: comparer,
                actions: actions);
    }
}
