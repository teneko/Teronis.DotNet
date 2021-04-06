// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Specialized;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class SortedCollectionModifications
    {
        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="collectionOrder"/>.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="collectionOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <param name="yieldCapabilities">The yieldCapabilities that regulates how <paramref name="leftItems"/> and <paramref name="rightItems"/> are synchronized.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            Func<TLeftItem, TComparablePart> getComparablePartOfLeftItem,
            IEnumerable<TRightItem> rightItems,
            Func<TRightItem, TComparablePart> getComparablePartOfRightItem,
            SortedCollectionOrder collectionOrder,
            IComparer<TComparablePart> comparer,
            CollectionModificationsYieldCapabilities yieldCapabilities)
        {
            var canInsert = yieldCapabilities.HasFlag(CollectionModificationsYieldCapabilities.Insert);
            var canRemove = yieldCapabilities.HasFlag(CollectionModificationsYieldCapabilities.Remove);
            var canReplace = yieldCapabilities.HasFlag(CollectionModificationsYieldCapabilities.Replace);

            var leftIndexDirectory = new IndexDirectory();

            var leftItemsEnumerator = new IndexPreferredEnumerator<TLeftItem>(leftItems, () => leftIndexDirectory.Count - 1);
            bool leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();

            var rightItemsEnumerator = rightItems.GetEnumerator();
            bool rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();

            int leftIndexOfLatestSyncedRightItem = -1;

            while (leftItemsEnumeratorIsFunctional || rightItemsEnumeratorIsFunctional) {
                if (!rightItemsEnumeratorIsFunctional) {
                    if (canRemove) {
                        var leftItem = leftItemsEnumerator.Current;

                        yield return CollectionModification<TRightItem, TLeftItem>.CreateOld(
                                NotifyCollectionChangedAction.Remove,
                                leftItem,
                                leftIndexOfLatestSyncedRightItem + 1);
                    } else {
                        leftIndexDirectory.Expand(leftIndexDirectory.Count);
                        leftIndexOfLatestSyncedRightItem++;
                    }

                    leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                } else if (!leftItemsEnumeratorIsFunctional) {
                    if (canInsert) {
                        var rightItem = rightItemsEnumerator.Current;
                        leftIndexOfLatestSyncedRightItem++;

                        yield return CollectionModification<TRightItem, TLeftItem>.CreateNew(
                            NotifyCollectionChangedAction.Add,
                            rightItem,
                            leftIndexOfLatestSyncedRightItem);

                        leftIndexDirectory.Expand(leftIndexDirectory.Count);
                    }

                    rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                } else {
                    var leftItem = leftItemsEnumerator.Current;
                    var rightItem = rightItemsEnumerator.Current;

                    var comparablePartOfLeftItem = getComparablePartOfLeftItem(leftItem);
                    var comparablePartOfRightItem = getComparablePartOfRightItem(rightItem);

                    var comparablePartComparison = collectionOrder == SortedCollectionOrder.Ascending
                        ? comparer.Compare(comparablePartOfLeftItem, comparablePartOfRightItem)
                        : comparer.Compare(comparablePartOfRightItem, comparablePartOfLeftItem);

                    if (comparablePartComparison < 0) {
                        if (canRemove) {
                            yield return CollectionModification<TRightItem, TLeftItem>.CreateOld(
                                NotifyCollectionChangedAction.Remove,
                                leftItem,
                                leftIndexOfLatestSyncedRightItem + 1);
                        } else {
                            leftIndexDirectory.Expand(leftIndexDirectory.Count);
                            leftIndexOfLatestSyncedRightItem++;
                        }

                        leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                    } else if (comparablePartComparison > 0) {
                        if (canInsert) {
                            leftIndexOfLatestSyncedRightItem++;

                            yield return CollectionModification<TRightItem, TLeftItem>.CreateNew(
                            NotifyCollectionChangedAction.Add,
                            rightItem,
                            leftIndexOfLatestSyncedRightItem);

                            leftIndexDirectory.Expand(leftIndexDirectory.Count);
                        }

                        rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                    } else {
                        leftIndexOfLatestSyncedRightItem++;

                        if (canReplace) {

                            yield return new CollectionModification<TRightItem, TLeftItem>(
                                NotifyCollectionChangedAction.Replace,
                                leftItem,
                                leftIndexOfLatestSyncedRightItem,
                                rightItem,
                                leftIndexOfLatestSyncedRightItem);
                        }

                        leftIndexDirectory.Expand(leftIndexDirectory.Count);
                        leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                        rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
                    }
                }
            }
        }

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="collectionOrder"/>.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="collectionOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            Func<TLeftItem, TComparablePart> getComparablePartOfLeftItem,
            IEnumerable<TRightItem> rightItems,
            Func<TRightItem, TComparablePart> getComparablePartOfRightItem,
            SortedCollectionOrder collectionOrder,
            IComparer<TComparablePart> comparer) =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                collectionOrder,
                comparer,
                CollectionModificationsYieldCapabilities.All);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="collectionOrder"/>.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="collectionOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="yieldCapabilities">The yieldCapabilities that regulates how <paramref name="leftItems"/> and <paramref name="rightItems"/> are synchronized.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            Func<TLeftItem, TComparablePart> getComparablePartOfLeftItem,
            IEnumerable<TRightItem> rightItems,
            Func<TRightItem, TComparablePart> getComparablePartOfRightItem,
            SortedCollectionOrder collectionOrder,
            CollectionModificationsYieldCapabilities yieldCapabilities) =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                collectionOrder,
                Comparer<TComparablePart>.Default,
                yieldCapabilities);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="collectionOrder"/>.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="collectionOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <param name="yieldCapabilities">The yieldCapabilities that regulates how <paramref name="leftItems"/> and <paramref name="rightItems"/> are synchronized.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TItem, TItem>> YieldCollectionModifications<TItem>(
            IEnumerable<TItem> leftItems,
            IEnumerable<TItem> rightItems,
            SortedCollectionOrder collectionOrder,
            IComparer<TItem> comparer,
            CollectionModificationsYieldCapabilities yieldCapabilities) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                collectionOrder,
                comparer: comparer,
                yieldCapabilities: yieldCapabilities);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="collectionOrder"/>.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="collectionOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="comparer">The comparer to be used to compare comparable parts of left and right item.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TItem, TItem>> YieldCollectionModifications<TItem>(
            IEnumerable<TItem> leftItems,
            IEnumerable<TItem> rightItems,
            SortedCollectionOrder collectionOrder,
            IComparer<TItem> comparer) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                collectionOrder,
                comparer,
                CollectionModificationsYieldCapabilities.All);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// Assumes <paramref name="leftItems"/> and <paramref name="rightItems"/> to be sorted by that order you specify by <paramref name="collectionOrder"/>.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="collectionOrder">the presumed order of items to be used to determine <see cref="IComparer{T}.Compare(T, T)"/> argument assignment.</param>
        /// <param name="yieldCapabilities">The yieldCapabilities that regulates how <paramref name="leftItems"/> and <paramref name="rightItems"/> are synchronized.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TItem, TItem>> YieldCollectionModifications<TItem>(
            IEnumerable<TItem> leftItems,
            IEnumerable<TItem> rightItems,
            SortedCollectionOrder collectionOrder,
            CollectionModificationsYieldCapabilities yieldCapabilities) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                collectionOrder,
                Comparer<TItem>.Default,
                yieldCapabilities);
    }
}
