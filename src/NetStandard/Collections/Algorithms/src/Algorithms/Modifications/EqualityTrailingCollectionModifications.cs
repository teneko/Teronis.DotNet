// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Collections.Specialized;

namespace Teronis.Collections.Algorithms.Modifications
{
    /// <summary>
    /// The algorithm creates modifications that can transform one collection into another collection.
    /// If equal left and right items are appearing the right items are going to act as markers. The
    /// right items before markers are the children of the markers, to assure that the these right
    /// items are inserted before their individual marker.
    /// </summary>
    public static partial class EqualityTrailingCollectionModifications
    {
        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftComparands"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftComparand">The type of left items.</typeparam>
        /// <typeparam name="TRightComparand">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer">The equality comparer to be used to compare comparable parts.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        private static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModificationsCore<TLeftItem, TRightItem, TLeftComparand, TRightComparand, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem> rightItems,
            IEnumerable<TLeftComparand> leftComparands,
            IEnumerable<TRightComparand> rightComparands,
            IEqualityComparer<TComparablePart>? equalityComparer,
            CollectionModificationYieldCapabilities yieldCapabilities)
            where TLeftComparand : TComparablePart
            where TRightComparand : TComparablePart
            where TComparablePart : notnull
        {
            static CollectionModification<TRightItem, TLeftItem> CreateReplaceModification(
                LinkedBucketListNode<TComparablePart, LeftItemContainer<TLeftItem>> leftComparandNode,
                LinkedBucketListNode<TComparablePart, RightItemContainer<TLeftItem, TRightItem>> rightComparandNode) =>
                CollectionModification.ForReplace(
                    leftComparandNode.Value.IndexEntry,
                    leftComparandNode.Value.Item,
                    rightComparandNode.Value.Item);

            static CollectionModification<TRightItem, TLeftItem> CreateMoveModification(
                LinkedBucketListNode<TComparablePart, LeftItemContainer<TLeftItem>> leftComparandNode,
                int leftComparandMoveToIndex,
                LinkedBucketListNode<TComparablePart, RightItemContainer<TLeftItem, TRightItem>> rightComparandNode) =>
                CollectionModification.ForMove<TRightItem, TLeftItem>(
                    leftComparandNode.Value.IndexEntry,
                    leftComparandNode.Value.Item,
                    leftComparandMoveToIndex);

            equalityComparer = equalityComparer ?? EqualityComparer<TComparablePart>.Default;

            var canInsert = yieldCapabilities.HasFlag(CollectionModificationYieldCapabilities.Insert);
            var canRemove = yieldCapabilities.HasFlag(CollectionModificationYieldCapabilities.Remove);
            var canReplace = yieldCapabilities.HasFlag(CollectionModificationYieldCapabilities.Replace);
            var canMove = canInsert && canRemove;

            var leftIndexDirectory = new IndexDirectory();

            var leftComparandsEnumerator = new IndexPreferredEnumerator<TLeftComparand>(leftComparands, () => leftIndexDirectory.Count - 1);
            var leftComparandsNodes = new LinkedBucketList<TComparablePart, LeftItemContainer<TLeftItem>>(equalityComparer);
            var leftItemsEnumerator = new IndexPreferredEnumerator<TLeftItem>(leftItems, () => leftIndexDirectory.Count - 1);
            var leftEnumeratorsAreFunctional = leftComparandsEnumerator.MoveNext() && leftItemsEnumerator.MoveNext();

            var rightComparandsEnumerator = rightComparands.GetEnumerator();
            var rightComparandIndexNext = 0;
            var rightComparandsNodes = new LinkedBucketList<TComparablePart, RightItemContainer<TLeftItem, TRightItem>>(equalityComparer);
            var rightItemsEnumerator = rightItems.GetEnumerator();
            var rightEnumeratorsAreFunctional = rightComparandsEnumerator.MoveNext() && rightItemsEnumerator.MoveNext();

            var leftIndexOfLatestSyncedRightItem = new IndexDirectoryEntry(-1, IndexDirectoryEntryMode.Floating);

            void SetLeftIndexOfLatestSyncedRightItem(int newIndex)
            {
                if (newIndex > leftIndexOfLatestSyncedRightItem.Index) {
                    leftIndexDirectory.ReplaceEntry(leftIndexOfLatestSyncedRightItem, newIndex);
                }
            }

            while (leftEnumeratorsAreFunctional || rightEnumeratorsAreFunctional) {
                LinkedBucketListNode<TComparablePart, LeftItemContainer<TLeftItem>>? leftComparandNodeLast;
                LinkedBucketListNode<TComparablePart, RightItemContainer<TLeftItem, TRightItem>>? rightComparandNodeLast;

                if (rightEnumeratorsAreFunctional) {
                    var rightComparand = rightComparandsEnumerator.Current;

                    rightComparandNodeLast = rightComparandsNodes.AddLast(rightComparand, new RightItemContainer<TLeftItem, TRightItem>(rightItemsEnumerator.Current, rightComparandIndexNext));
                    rightComparandIndexNext++;
                } else {
                    rightComparandNodeLast = null;
                }

                var rightComparandNodeLastBucketFirstNode = rightComparandNodeLast?.Bucket!.First;

                /* Is the first node of bucket of right node anywhere on left side? */
                if (!(rightComparandNodeLastBucketFirstNode is null) && leftComparandsNodes.TryGetBucket(rightComparandNodeLastBucketFirstNode!.Key, out var leftComparandBucket)) {
                    var leftComparandNode = leftComparandBucket.First!;

                    if (canReplace) {
                        yield return CreateReplaceModification(leftComparandNode, rightComparandNodeLastBucketFirstNode);
                    }

                    int leftComparandMoveToIndex;

                    if (leftComparandNode.Value.IndexEntry > leftIndexOfLatestSyncedRightItem.Index) {
                        // We do not need to move, because the item has not 
                        // exceeded the index of latest synced right item.
                        leftComparandMoveToIndex = leftComparandNode.Value.IndexEntry;
                    } else {
                        // The index where it would be inserted when it is removed.
                        leftComparandMoveToIndex = leftIndexOfLatestSyncedRightItem.Index;
                    }

                    LinkedBucketListNode<TComparablePart, RightItemContainer<TLeftItem, TRightItem>>? rightComparandNodeLastBucketFirstNodeListPreviousNode;

                    {
                        if (canMove && leftComparandNode.Value.IndexEntry != leftComparandMoveToIndex) {
                            var moveModification = CreateMoveModification(leftComparandNode, leftComparandMoveToIndex, rightComparandNodeLastBucketFirstNode);
                            yield return moveModification;
                            leftIndexDirectory.Move(moveModification.OldIndex, moveModification.NewIndex);
                        }

                        rightComparandNodeLastBucketFirstNodeListPreviousNode = rightComparandNodeLastBucketFirstNode.ListPart.Previous;
                        rightComparandNodeLastBucketFirstNode.Bucket?.Remove(rightComparandNodeLastBucketFirstNode);

                        leftComparandNode.Bucket!.Remove(leftComparandNode);
                    }

                    var currentLeftItemNodeAsParentForPreviousRightItemNodes = leftComparandNode;

                    while (!(rightComparandNodeLastBucketFirstNodeListPreviousNode is null)) {
                        // Do not set parent after it has been already set.
                        if (rightComparandNodeLastBucketFirstNodeListPreviousNode.Value.Parent is null) {
                            // We cannot add right item blindly to left side, because we do not know if exact this node will
                            // appear on left side. So we tell current previous node about a left node that is definitely below him.
                            rightComparandNodeLastBucketFirstNodeListPreviousNode.Value.Parent = currentLeftItemNodeAsParentForPreviousRightItemNodes.Value;
                        }

                        var tempPrevious = rightComparandNodeLastBucketFirstNodeListPreviousNode.ListPart.Previous;

                        if (tempPrevious is null || !(tempPrevious.Value.Parent is null)) {
                            rightComparandNodeLastBucketFirstNodeListPreviousNode = null;
                        } else {
                            rightComparandNodeLastBucketFirstNodeListPreviousNode = tempPrevious;
                        }
                    }

                    // Let's refresh index after left item may have been moved.
                    SetLeftIndexOfLatestSyncedRightItem(leftComparandNode.Value.IndexEntry);
                }

                if (leftEnumeratorsAreFunctional) {
                    var leftComparand = leftComparandsEnumerator.Current;

                    var nextLeftIndex = leftIndexDirectory.Count;

                    leftComparandNodeLast = leftComparandsNodes.AddLast(leftComparand, new LeftItemContainer<TLeftItem>(leftItemsEnumerator.Current, leftIndexDirectory.Add(nextLeftIndex)));
                } else {
                    leftComparandNodeLast = null;
                }

                var leftComparandNodeLastBucketFirstNode = leftComparandNodeLast?.Bucket?.First;

                /* Is the first node of bucket of left node anywhere on right side? */
                if (!(leftComparandNodeLastBucketFirstNode is null) && rightComparandsNodes.TryGetBucket(leftComparandNodeLastBucketFirstNode.Key, out var rightComparandbucket)) {
                    var rightComparandNode = rightComparandbucket.First!;

                    if (canReplace) {
                        yield return CreateReplaceModification(leftComparandNodeLastBucketFirstNode, rightComparandNode);
                    }

                    if (canMove && !(rightComparandNode.Value.Parent is null)) {
                        var moveModification = CreateMoveModification(
                            leftComparandNodeLastBucketFirstNode,
                            rightComparandNode.Value.Parent.IndexEntry,
                            rightComparandNode);

                        yield return moveModification;
                        leftIndexDirectory.Move(moveModification.OldIndex, moveModification.NewIndex);
                    }

                    var rightComparandNodePrevious = rightComparandNode.ListPart.Previous;

                    while (!(rightComparandNodePrevious is null)
                        && (rightComparandNode.Value.Parent is null && rightComparandNodePrevious.Value.Parent is null
                            || !(rightComparandNode.Value.Parent is null) && ReferenceEquals(rightComparandNodePrevious.Value.Parent, rightComparandNode.Value.Parent))) {
                        rightComparandNodePrevious.Value.Parent = leftComparandNodeLastBucketFirstNode.Value;
                        rightComparandNodePrevious = rightComparandNodePrevious.ListPart.Previous;
                    }

                    leftComparandNodeLastBucketFirstNode.Bucket?.Remove(leftComparandNodeLastBucketFirstNode);
                    rightComparandNode.Bucket?.Remove(rightComparandNode);
                    SetLeftIndexOfLatestSyncedRightItem(leftComparandNodeLastBucketFirstNode.Value.IndexEntry);
                }

                leftEnumeratorsAreFunctional = leftComparandsEnumerator.MoveNext() && leftItemsEnumerator.MoveNext();
                rightEnumeratorsAreFunctional = rightComparandsEnumerator.MoveNext() && rightItemsEnumerator.MoveNext();
            }

            var leftComparandsLength = leftComparandsEnumerator.CurrentLength;

            if (canRemove && !(leftComparandsNodes.Last is null)) {
                foreach (var leftComparandNode in LinkedBucketListUtils.YieldNodesReversed(leftComparandsNodes.Last)) {
                    var removeModification = CollectionModification.ForRemove<TRightItem, TLeftItem>(leftComparandNode.Value.IndexEntry, leftComparandNode.Value.Item);
                    yield return removeModification;

                    leftIndexDirectory.Remove(removeModification.OldIndex);
                    leftComparandsLength--;
                }
            }

            if (canInsert && !(rightComparandsNodes.First is null)) {
                foreach (var rightComparandNode in rightComparandsNodes) {
                    int insertItemTo;

                    if (rightComparandNode.Parent is null) {
                        insertItemTo = leftComparandsLength;
                    } else {
                        insertItemTo = rightComparandNode.Parent.IndexEntry;
                    }

                    var addModification = CollectionModification.ForAdd<TRightItem, TLeftItem>(insertItemTo, rightComparandNode.Item);
                    yield return addModification;

                    leftIndexDirectory.Insert(insertItemTo);
                    leftComparandsLength++;
                }
            }
        }

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer">The equality comparer to be used to compare comparable parts.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TLeftComparand, TRightComparand, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem> rightItems,
            IEnumerable<TLeftComparand> leftComparands,
            IEnumerable<TRightComparand> rightComparands,
            IEqualityComparer<TComparablePart>? equalityComparer,
            CollectionModificationYieldCapabilities yieldCapabilities)
            where TLeftComparand : TComparablePart
            where TRightComparand : TComparablePart
            where TComparablePart : notnull =>
            YieldCollectionModificationsCore(
                leftItems,
                rightItems,
                leftComparands,
                rightComparands,
                equalityComparer,
                yieldCapabilities);


        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer">The equality comparer to be used to compare comparable parts.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TLeftComparand, TRightComparand, TComparablePart>(
        IEnumerable<TLeftItem> leftItems,
        IEnumerable<TRightItem> rightItems,
        IEnumerable<TLeftComparand> leftComparands,
        IEnumerable<TRightComparand> rightComparands,
        IEqualityComparer<TComparablePart>? equalityComparer)
        where TLeftComparand : TComparablePart
        where TRightComparand : TComparablePart
        where TComparablePart : notnull =>
        YieldCollectionModifications(
            leftItems,
            rightItems,
            leftComparands,
            rightComparands,
            equalityComparer,
            CollectionModificationYieldCapabilities.All);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TLeftComparand, TRightComparand, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem> rightItems,
            IEnumerable<TLeftComparand> leftComparands,
            IEnumerable<TRightComparand> rightComparands,
            CollectionModificationYieldCapabilities yieldCapabilities)
            where TLeftComparand : TComparablePart
            where TRightComparand : TComparablePart
            where TComparablePart : notnull =>
            YieldCollectionModifications(
                leftItems,
                rightItems,
                leftComparands,
                rightComparands,
                EqualityComparer<TComparablePart>.Default,
                yieldCapabilities);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TLeftComparand, TRightComparand, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem> rightItems,
            IEnumerable<TLeftComparand> leftComparands,
            IEnumerable<TRightComparand> rightComparands)
            where TLeftComparand : TComparablePart
            where TRightComparand : TComparablePart
            where TComparablePart : notnull =>
            YieldCollectionModifications(
                leftItems,
                rightItems,
                leftComparands,
                rightComparands,
                EqualityComparer<TComparablePart>.Default,
                CollectionModificationYieldCapabilities.All);

        private class LeftItemContainer<TLeftItem>
        {
            public TLeftItem Item { get; }
            public IndexDirectoryEntry IndexEntry { get; }

            public LeftItemContainer(TLeftItem item, IndexDirectoryEntry indexEntry)
            {
                Item = item;
                IndexEntry = indexEntry ?? throw new ArgumentNullException(nameof(indexEntry));
            }
        }

        private class RightItemContainer<TLeftItem, TRightItem>
        {
            public TRightItem Item { get; }
            /// <summary>
            /// Not null means that this right item should be BEFORE parent.
            /// </summary>
            public LeftItemContainer<TLeftItem>? Parent { get; set; }
            public int Index { get; }

            public RightItemContainer(TRightItem item, int index)
            {
                Item = item;
                Index = index;
            }
        }
    }
}
