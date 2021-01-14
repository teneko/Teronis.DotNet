using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Changes
{
    public static class SortedCollectionModifications
    {
        /// <summary>
        /// Yields collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted in ascending or descending order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="LeftItemType">The typ of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="assumedOrder">the assumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            SortedCollectionModificationsOrder assumedOrder,
            IComparer<ComparablePartType> comparer)
        {
            comparer = comparer ?? Comparer<ComparablePartType>.Default;
            var leftItemsEnumerator = leftItems.GetEnumerator();
            var rightItemsEnumerator = rightItems.GetEnumerator();
            bool leftItemsEnumeratorIsFunctional, rightItemsEnumeratorIsFunctional;
            int leftItemIndex = 0, rightItemIndex = 0;

            leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
            rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();

            while (leftItemsEnumeratorIsFunctional || rightItemsEnumeratorIsFunctional) {
                if (!rightItemsEnumeratorIsFunctional) {
                    var leftItem = leftItemsEnumerator.Current;

                    yield return CollectionModification<LeftItemType, RightItemType>.CreateOld(
                            NotifyCollectionChangedAction.Remove,
                            leftItem,
                            leftItemIndex);

                    leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                    leftItemIndex++;
                } else if (!leftItemsEnumeratorIsFunctional) {
                    var rightItem = rightItemsEnumerator.Current;

                    yield return CollectionModification<LeftItemType, RightItemType>.CreateNew(
                            NotifyCollectionChangedAction.Add,
                            rightItem,
                            rightItemIndex);

                    rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                    rightItemIndex++;
                } else {
                    var leftItem = leftItemsEnumerator.Current;
                    var rightItem = rightItemsEnumerator.Current;

                    var comparablePartOfLeftItem = getComparablePartOfLeftItem(leftItem);
                    var comparablePartOfRightItem = getComparablePartOfRightItem(rightItem);

                    var comparablePartComparison = assumedOrder == SortedCollectionModificationsOrder.Ascending
                        ? comparer.Compare(comparablePartOfLeftItem, comparablePartOfRightItem)
                        : comparer.Compare(comparablePartOfRightItem, comparablePartOfLeftItem);

                    if (comparablePartComparison < 0) {
                        yield return CollectionModification<LeftItemType, RightItemType>.CreateOld(
                            NotifyCollectionChangedAction.Remove,
                            leftItem,
                            leftItemIndex);

                        leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                        leftItemIndex++;
                    } else if (comparablePartComparison > 0) {
                        yield return CollectionModification<LeftItemType, RightItemType>.CreateNew(
                            NotifyCollectionChangedAction.Add,
                            rightItem,
                            rightItemIndex);

                        rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                        rightItemIndex++;
                    } else {
                        yield return new CollectionModification<LeftItemType, RightItemType>(
                            NotifyCollectionChangedAction.Replace,
                            leftItem,
                            leftItemIndex,
                            rightItem,
                            leftItemIndex);

                        leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                        leftItemIndex++;

                        rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                        rightItemIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Yields collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted in ascending or descending order.
        /// descending order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="LeftItemType">The typ of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="assumedOrder">the assumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            SortedCollectionModificationsOrder assumedOrder) =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                assumedOrder);

        /// <summary>
        /// Yields collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted in ascending or descending order.
        /// descending order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="ItemType">The typ of left and right items.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="assumedOrder">the assumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            SortedCollectionModificationsOrder assumedOrder,
            IComparer<ItemType> comparer) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                assumedOrder, comparer);

        /// <summary>
        /// Yields collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted in ascending or descending order.
        /// descending order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="ItemType">The typ of left and right items.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="assumedOrder">the assumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            SortedCollectionModificationsOrder assumedOrder) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                assumedOrder, 
                Comparer<ItemType>.Default);
    }
}
