using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Specialized;

namespace Teronis.Collections.Changes
{
    public static class CollectionModifications
    {
        /// <summary>
        /// Yields the collection modifications for <paramref name="leftItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="LeftItemType">The typ of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <returns>The collection modifications for <paramref name="leftItems"/></returns>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            IEqualityComparer<ComparablePartType>? equalityComparer = null,
            CollectionModificationsActions actions = CollectionModificationsActions.All)
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

            var canInsert = actions.HasFlag(CollectionModificationsActions.Insert);
            var canRemove = actions.HasFlag(CollectionModificationsActions.Remove);

            var leftIndexDirectory = new IndexDirectory();

            var leftItemsEnumerator = new IndexPreferredEnumerator<LeftItemType>(leftItems, leftIndexDirectory);
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

                    yield return createReplaceModification(leftItemNode, rightItemNodeLastBucketFirstNode);

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
                        if (leftItemNode.Value.IndexEntry != leftItemMoveToIndex) {
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
                        if (leftItemsNodes.TryGetBucket(rightItemNodeLastBucketFirstNodeListPreviousNode.Key, out var leftBucket)) {
                            var leftItemFirstNodeOfRightItemNodeLastBucketFirstNodeListPreviousNode = leftBucket.First!;

                            yield return createReplaceModification(
                                leftItemFirstNodeOfRightItemNodeLastBucketFirstNodeListPreviousNode,
                                rightItemNodeLastBucketFirstNodeListPreviousNode);

                            int newLeftItemMoveToIndex;

                            if (!(rightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent is null)) {
                                // If the parent is not null, we want to move it before that parent.
                                newLeftItemMoveToIndex = rightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent.IndexEntry;
                                rightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent = null;
                            } else {
                                newLeftItemMoveToIndex = leftItemMoveToIndex;
                            }

                            var moveModification = createMoveModification(
                                leftItemFirstNodeOfRightItemNodeLastBucketFirstNodeListPreviousNode,
                                newLeftItemMoveToIndex,
                                rightItemNodeLastBucketFirstNodeListPreviousNode);

                            yield return moveModification;

                            leftItemFirstNodeOfRightItemNodeLastBucketFirstNodeListPreviousNode.Bucket!.Remove(leftItemFirstNodeOfRightItemNodeLastBucketFirstNodeListPreviousNode);
                            leftIndexDirectory.Move(moveModification.OldIndex, moveModification.NewIndex);

                            // After dispatch we set new index;
                            currentLeftItemNodeAsParentForPreviousRightItemNodes = leftItemFirstNodeOfRightItemNodeLastBucketFirstNodeListPreviousNode;
                        } else {
                            // Do not set parent after it has been already set.
                            if (rightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent is null) {
                                // We cannot add right item blindly to left side, because we do not know if exact this node will
                                // appear on left side. So we tell current previous node about a left node that is definitely below him.
                                rightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent = currentLeftItemNodeAsParentForPreviousRightItemNodes.Value;
                            }
                        }

                        var tempRightItemNodeLastBucketFirstNodeListPreviousNode = rightItemNodeLastBucketFirstNodeListPreviousNode;
                        var tempPrevious = rightItemNodeLastBucketFirstNodeListPreviousNode.ListPart.Previous;

                        if (tempPrevious is null || !(tempPrevious.Value.Parent is null)) {
                            rightItemNodeLastBucketFirstNodeListPreviousNode = null;
                        } else {
                            rightItemNodeLastBucketFirstNodeListPreviousNode = tempPrevious;
                        }

                        if (tempRightItemNodeLastBucketFirstNodeListPreviousNode.Value.Parent is null) {
                            // Remove node that has been definitely found on left side.
                            tempRightItemNodeLastBucketFirstNodeListPreviousNode.Bucket?.Remove(tempRightItemNodeLastBucketFirstNodeListPreviousNode);
                        }
                    }

                    // Let's refresh index after left item may have been moved several times.
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

                    yield return createReplaceModification(leftItemNodeLastBucketFirstNode, rightItemNode);

                    if (!(rightItemNode.Value.Parent is null)) {
                        var moveLeftItemTo = rightItemNode.Value.Parent.IndexEntry;

                        var moveModification = createMoveModification(
                            leftItemNodeLastBucketFirstNode,
                            moveLeftItemTo,
                            rightItemNode);

                        yield return moveModification;
                        leftIndexDirectory.Move(moveModification.OldIndex, moveModification.NewIndex);
                    }

                    var rightItemNodePrevious = rightItemNode.ListPart.Previous;

                    while (!(rightItemNodePrevious is null)
                        && ((rightItemNode.Value.Parent is null && rightItemNodePrevious.Value.Parent is null)
                            || (!(rightItemNode.Value.Parent is null) && ReferenceEquals(rightItemNodePrevious.Value.Parent, rightItemNode.Value.Parent)))) {
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
        /// Yields the collection modifications for <paramref name="leftItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="LeftItemType">The typ of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <returns>The collection modifications for <paramref name="leftItems"/></returns>
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
                EqualityComparer<ComparablePartType>.Default);

        /// <summary>
        /// Yields the collection modifications for <paramref name="leftItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <returns>The collection modifications for <paramref name="leftItems"/></returns>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            IEqualityComparer<ItemType> equalityComparer)
            where ItemType : notnull =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                equalityComparer);

        /// <summary>
        /// Yields the collection modifications for <paramref name="leftItems"/>.
        /// The collection modifications may be used to reorder <paramref name="leftItems"/>.
        /// The more the collection is synchronized in an orderly way, the more efficient the algorithm is.
        /// Duplications are allowed but take into account that duplications are yielded as they are appearing.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="leftItems">The left items to whom collection modifications are addressed to.</param>
        /// <param name="rightItems">The right items that left items want to become.</param>
        /// <returns>The collection modifications for <paramref name="leftItems"/></returns>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems)
            where ItemType : notnull =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                EqualityComparer<ItemType>.Default);

        private class IndexPreferredEnumerator<ItemType> : IEnumerator<ItemType>
        {
            public int CurrentLength { get; private set; }

            private readonly IEnumerator<ItemType> enumerator;

            public IndexPreferredEnumerator(IEnumerable<ItemType> enumerable, IndexDirectory leftIndexDirectory)
            {
                if (enumerable is IReadOnlyList<ItemType> readOnlyList) {
                    enumerator = new IndexedEnumerator(readOnlyList, leftIndexDirectory);
                } else if (enumerable is IList<ItemType> list) {
                    enumerator = new IndexedEnumerator(new ReadOnlyCollection<ItemType>(list), leftIndexDirectory);
                } else {
                    enumerator = enumerable.GetEnumerator();
                }
            }

            public ItemType Current =>
                enumerator.Current;

            public bool MoveNext()
            {
                if (enumerator.MoveNext()) {
                    CurrentLength++;
                    return true;
                } else {
                    return false;
                }
            }

            public void Reset() =>
                enumerator.Reset();

            object? IEnumerator.Current =>
                ((IEnumerator)enumerator).Current;

            public void Dispose() =>
                enumerator.Dispose();

            public class IndexedEnumerator : IEnumerator<ItemType>
            {
                private readonly IReadOnlyList<ItemType> list;
                private readonly IndexDirectory leftIndexDirectory;

                public ItemType Current { get; private set; }

                public IndexedEnumerator(IReadOnlyList<ItemType> list, IndexDirectory leftIndexDirectory)
                {
                    Current = default!;
                    this.list = list;
                    this.leftIndexDirectory = leftIndexDirectory;
                }

                public bool MoveNext()
                {
                    if (leftIndexDirectory.Count < list.Count) {
                        Current = list[leftIndexDirectory.Count];
                        return true;
                    } else {
                        return false;
                    }
                }

                public void Reset() =>
                    throw new NotImplementedException();

                object IEnumerator.Current =>
                    Current!;

                public void Dispose() { }
            }
        }

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
