using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="LeftItemType">The type of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="equalityComparer">The equality comparer to be used to compare comparable parts.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            IEqualityComparer<ComparablePartType>? equalityComparer,
            CollectionModificationsYieldCapabilities yieldCapabilities)
            where ComparablePartType : notnull
        {
            static CollectionModification<LeftItemType, RightItemType> createReplaceModification(
                LinkedBucketListNode<ComparablePartType, LeftItemContainer<LeftItemType>> leftItemNode,
                LinkedBucketListNode<ComparablePartType, RightItemContainer<LeftItemType, RightItemType>> rightItemNode) =>
                new CollectionModification<LeftItemType, RightItemType>(
                    NotifyCollectionChangedAction.Replace,
                    leftItemNode.Value.Item,
                    leftItemNode.Value.IndexEntry,
                    rightItemNode.Value.Item,
                    leftItemNode.Value.IndexEntry);

            static CollectionModification<LeftItemType, RightItemType> createMoveModification(
                LinkedBucketListNode<ComparablePartType, LeftItemContainer<LeftItemType>> leftItemNode,
                int leftItemMoveToIndex,
                LinkedBucketListNode<ComparablePartType, RightItemContainer<LeftItemType, RightItemType>> rightItemNode) =>
                new CollectionModification<LeftItemType, RightItemType>(
                    NotifyCollectionChangedAction.Move,
                    leftItemNode.Value.Item,
                    leftItemNode.Value.IndexEntry,
                    rightItemNode.Value.Item,
                    leftItemMoveToIndex);

            equalityComparer = equalityComparer ?? EqualityComparer<ComparablePartType>.Default;

            var canInsert = yieldCapabilities.HasFlag(CollectionModificationsYieldCapabilities.Insert);
            var canRemove = yieldCapabilities.HasFlag(CollectionModificationsYieldCapabilities.Remove);
            var canReplace = yieldCapabilities.HasFlag(CollectionModificationsYieldCapabilities.Replace);
            var canMove = canInsert && canRemove;

            var leftIndexDirectory = new IndexDirectory();

            var leftItemsEnumerator = new IndexPreferredEnumerator<LeftItemType>(leftItems, leftIndexDirectory.LastIndexEntry);
            var leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
            var leftItemsNodes = new LinkedBucketList<ComparablePartType, LeftItemContainer<LeftItemType>>(equalityComparer);

            var rightItemsEnumerator = rightItems.GetEnumerator();
            var rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
            var rightItemIndexNext = 0;
            var rightItemsNodes = new LinkedBucketList<ComparablePartType, RightItemContainer<LeftItemType, RightItemType>>(equalityComparer);

            var leftIndexOfLatestSyncedRightItem = new IndexDirectoryEntry(-1, IndexDirectoryEntryMode.Floating);

            void SetLeftIndexOfLatestSyncedRightItem(int newIndex)
            {
                if (newIndex > leftIndexOfLatestSyncedRightItem.Index) {
                    leftIndexDirectory.ReplaceEntry(leftIndexOfLatestSyncedRightItem, newIndex);
                }
            }

            while (leftItemsEnumeratorIsFunctional || rightItemsEnumeratorIsFunctional) {
                LinkedBucketListNode<ComparablePartType, LeftItemContainer<LeftItemType>>? leftItemNodeLast;
                LinkedBucketListNode<ComparablePartType, RightItemContainer<LeftItemType, RightItemType>>? rightItemNodeLast;

                if (rightItemsEnumeratorIsFunctional) {
                    var rightItem = rightItemsEnumerator.Current;
                    var comparablePartOfRightItem = getComparablePartOfRightItem(rightItem);

                    rightItemNodeLast = rightItemsNodes.AddLast(comparablePartOfRightItem, new RightItemContainer<LeftItemType, RightItemType>(rightItem, rightItemIndexNext));
                    rightItemIndexNext++;
                } else {
                    rightItemNodeLast = null;
                }

                var rightItemNodeLastBucketFirstNode = rightItemNodeLast?.Bucket!.First;

                /* Is the first node of bucket of right node anywhere on left side? */
                if (!(rightItemNodeLastBucketFirstNode is null) && leftItemsNodes.TryGetBucket(rightItemNodeLastBucketFirstNode!.Key, out var leftItemBucket)) {
                    var leftItemNode = leftItemBucket.First!;

                    if (canReplace) {
                        yield return createReplaceModification(leftItemNode, rightItemNodeLastBucketFirstNode);
                    }

                    int leftItemMoveToIndex;

                    if (leftItemNode.Value.IndexEntry > leftIndexOfLatestSyncedRightItem.Index) {
                        // We do not need to move, because the item has not 
                        // exceeded the index of latest synced right item.
                        leftItemMoveToIndex = leftItemNode.Value.IndexEntry;
                    } else {
                        // The index where it would be inserted when it is removed.
                        leftItemMoveToIndex = leftIndexOfLatestSyncedRightItem.Index;
                    }

                    LinkedBucketListNode<ComparablePartType, RightItemContainer<LeftItemType, RightItemType>>? rightItemNodeLastBucketFirstNodeListPreviousNode;

                    {
                        if (canMove && leftItemNode.Value.IndexEntry != leftItemMoveToIndex) {
                            var moveModification = createMoveModification(leftItemNode, leftItemMoveToIndex, rightItemNodeLastBucketFirstNode);
                            yield return moveModification;
                            leftIndexDirectory.Move(moveModification.OldIndex, moveModification.NewIndex);
                        }

                        rightItemNodeLastBucketFirstNodeListPreviousNode = rightItemNodeLastBucketFirstNode.ListPart.Previous;
                        rightItemNodeLastBucketFirstNode.Bucket?.Remove(rightItemNodeLastBucketFirstNode);

                        leftItemNode.Bucket!.Remove(leftItemNode);
                    }

                    var currentLeftItemNodeAsParentForPreviousRightItemNodes = leftItemNode;

                    while (!(rightItemNodeLastBucketFirstNodeListPreviousNode is null)) {
                        // Do not set parent after it has been already set.
                        if (rightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent is null) {
                            // We cannot add right item blindly to left side, because we do not know if exact this node will
                            // appear on left side. So we tell current previous node about a left node that is definitely below him.
                            rightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent = currentLeftItemNodeAsParentForPreviousRightItemNodes.Value;
                        }

                        var tempPrevious = rightItemNodeLastBucketFirstNodeListPreviousNode.ListPart.Previous;

                        if (tempPrevious is null || !(tempPrevious.Value.Parent is null)) {
                            rightItemNodeLastBucketFirstNodeListPreviousNode = null;
                        } else {
                            rightItemNodeLastBucketFirstNodeListPreviousNode = tempPrevious;
                        }
                    }

                    // Let's refresh index after left item may have been moved.
                    SetLeftIndexOfLatestSyncedRightItem(leftItemNode.Value.IndexEntry);
                }

                if (leftItemsEnumeratorIsFunctional) {
                    var leftItem = leftItemsEnumerator.Current;
                    var comparablePartOfLeftItem = getComparablePartOfLeftItem(leftItem);
                    var nextLeftIndex = leftIndexDirectory.Count;

                    leftItemNodeLast = leftItemsNodes.AddLast(comparablePartOfLeftItem, new LeftItemContainer<LeftItemType>(leftItem, leftIndexDirectory.Add(nextLeftIndex)));
                } else {
                    leftItemNodeLast = null;
                }

                var leftItemNodeLastBucketFirstNode = leftItemNodeLast?.Bucket?.First;

                /* Is the first node of bucket of left node anywhere on right side? */
                if (!(leftItemNodeLastBucketFirstNode is null) && rightItemsNodes.TryGetBucket(leftItemNodeLastBucketFirstNode!.Key, out var rightItembucket)) {
                    var rightItemNode = rightItembucket.First!;

                    if (canReplace) {
                        yield return createReplaceModification(leftItemNodeLastBucketFirstNode, rightItemNode);
                    }

                    if (canMove && !(rightItemNode.Value.Parent is null)) {
                        var moveModification = createMoveModification(
                            leftItemNodeLastBucketFirstNode,
                            rightItemNode.Value.Parent.IndexEntry,
                            rightItemNode);

                        yield return moveModification;
                        leftIndexDirectory.Move(moveModification.OldIndex, moveModification.NewIndex);
                    }

                    var rightItemNodePrevious = rightItemNode.ListPart.Previous;

                    while (!(rightItemNodePrevious is null)
                        && (rightItemNode.Value.Parent is null && rightItemNodePrevious.Value.Parent is null
                            || !(rightItemNode.Value.Parent is null) && ReferenceEquals(rightItemNodePrevious.Value.Parent, rightItemNode.Value.Parent))) {
                        rightItemNodePrevious.Value.Parent = leftItemNodeLastBucketFirstNode.Value;
                        rightItemNodePrevious = rightItemNodePrevious.ListPart.Previous;
                    }

                    leftItemNodeLastBucketFirstNode.Bucket?.Remove(leftItemNodeLastBucketFirstNode);
                    rightItemNode.Bucket?.Remove(rightItemNode);
                    SetLeftIndexOfLatestSyncedRightItem(leftItemNodeLastBucketFirstNode.Value.IndexEntry);
                }

                leftItemsEnumeratorIsFunctional = leftItemsEnumerator.MoveNext();
                rightItemsEnumeratorIsFunctional = rightItemsEnumerator.MoveNext();
            }

            var leftItemsLength = leftItemsEnumerator.CurrentLength;

            if (canRemove && !(leftItemsNodes.Last is null)) {
                foreach (var leftItemNode in LinkedBucketListUtils.YieldNodesReversed(leftItemsNodes.Last)) {
                    var removeModification = CollectionModification<LeftItemType, RightItemType>.CreateOld(
                        NotifyCollectionChangedAction.Remove,
                        leftItemNode.Value.Item,
                        leftItemNode.Value.IndexEntry);

                    yield return removeModification;
                    leftIndexDirectory.Remove(removeModification.OldIndex);
                    leftItemsLength--;
                }
            }

            if (canInsert && !(rightItemsNodes.First is null)) {
                foreach (var rightItemNode in rightItemsNodes) {
                    int insertItemTo;

                    if (rightItemNode.Parent is null) {
                        insertItemTo = leftItemsLength;
                    } else {
                        insertItemTo = rightItemNode.Parent.IndexEntry;
                    }

                    var addModification = CollectionModification<LeftItemType, RightItemType>.CreateNew(
                        NotifyCollectionChangedAction.Add,
                        rightItemNode.Item,
                        insertItemTo);

                    yield return addModification;
                    leftIndexDirectory.Insert(insertItemTo);
                    leftItemsLength++;
                }
            }
        }

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="LeftItemType">The type of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="equalityComparer">The equality comparer to be used to compare comparable parts.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            IEqualityComparer<ComparablePartType>? equalityComparer)
            where ComparablePartType : notnull =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                equalityComparer,
                CollectionModificationsYieldCapabilities.All);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="LeftItemType">The type of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            CollectionModificationsYieldCapabilities yieldCapabilities)
            where ComparablePartType : notnull =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                EqualityComparer<ComparablePartType>.Default,
                yieldCapabilities);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="LeftItemType">The type of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem)
            where ComparablePartType : notnull =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                EqualityComparer<ComparablePartType>.Default,
                CollectionModificationsYieldCapabilities.All);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer">The equality comparer to be used to compare comparable parts.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            IEqualityComparer<ItemType>? equalityComparer,
            CollectionModificationsYieldCapabilities yieldCapabilities)
            where ItemType : notnull =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                equalityComparer,
                yieldCapabilities);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer">The equality comparer to be used to compare comparable parts.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            IEqualityComparer<ItemType>? equalityComparer)
            where ItemType : notnull =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                equalityComparer,
                CollectionModificationsYieldCapabilities.All);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            CollectionModificationsYieldCapabilities yieldCapabilities)
            where ItemType : notnull =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                EqualityComparer<ItemType>.Default,
                yieldCapabilities);

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when non-nullable arguments are null.</exception>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems)
            where ItemType : notnull =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                EqualityComparer<ItemType>.Default,
                CollectionModificationsYieldCapabilities.All);

        private class ItemContainer<ItemType>
        {
            public ItemType Item { get; }

            public ItemContainer(ItemType item) =>
                Item = item;
        }

        private class LeftItemContainer<LeftItemType> : ItemContainer<LeftItemType>//, IDisposable
        {
            public IndexDirectoryEntry IndexEntry { get; set; }

            public LeftItemContainer(LeftItemType item, IndexDirectoryEntry indexEntry)
                : base(item) =>
                IndexEntry = indexEntry;
        }

        private class RightItemContainer<LeftItemType, RightItemType> : ItemContainer<RightItemType>
        {
            /// <summary>
            /// Not null means that this right item should be BEFORE parent.
            /// </summary>
            public LeftItemContainer<LeftItemType>? Parent { get; set; }
            public int Index { get; }

            public RightItemContainer(RightItemType item, int index)
                : base(item) =>
                Index = index;
        }

        private enum NodeAppendBehaviour
        {
            Before,
            After
        }
    }
}
