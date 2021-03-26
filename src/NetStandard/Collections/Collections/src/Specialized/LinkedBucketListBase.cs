// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    internal abstract partial class LinkedBucketListBase<KeyType, ValueType> : IEnumerable<ValueType>, ILinkedBucketList<KeyType, ValueType>
        where KeyType : notnull
    {
        internal static void ValidateNodeToBeAttached(LinkedBucketListBase<KeyType, ValueType> linkedListBucket, LinkedBucketListNode<KeyType, ValueType> node)
        {
            if (node is null) {
                throw new ArgumentNullException("The node is null.");
            }

            if (node.Bucket is null) {
                throw new ArgumentException("The bucket of node is null.");
            }

            if (!ReferenceEquals(node.Bucket.List, linkedListBucket.List)) {
                throw new InvalidOperationException("The node comes from foreign list.");
            }
        }

        internal static void ValidateNodeToBeNew(LinkedBucketListNode<KeyType, ValueType> node)
        {
            if (node is null) {
                throw new ArgumentNullException(nameof(node), "The node is null.");
            }

            if (!(node.Bucket is null)) {
                throw new ArgumentException("The node is already attached.");
            }
        }

        internal static void RemoveBucketListNode(LinkedBucketListNode<KeyType, ValueType> node, bool preserveEmptyBucket = false)
        {
            node.bucket.RemoveNode(node);
            node.bucket.list.RemoveNode(node);

            if (!preserveEmptyBucket && node.bucket.head is null) {
                node.bucket.list.buckets.Remove(node.Key);
                node.bucket.Invalidate();
            }

            node.Invalidate();
        }

        public ICovariantReadOnlyNullableKeyDictionary<KeyType, LinkedBucketListBase<KeyType, ValueType>> Buckets => buckets;
        ICovariantReadOnlyNullableKeyDictionary<KeyType, ILinkedBucketList<KeyType, ValueType>> ILinkedBucketList<KeyType, ValueType>.Buckets => Buckets;
        public LinkedBucketListBase<KeyType, ValueType> List => list ?? throw new InvalidOperationException("This bucket is not attached.");
        ILinkedBucketList<KeyType, ValueType> ILinkedBucketList<KeyType, ValueType>.List => List;
        public LinkedBucketListNode<KeyType, ValueType>? First => head;
        public LinkedBucketListNode<KeyType, ValueType>? Last => head?.ListPart.previous;
        public int Count => count;

        public abstract bool IsBucket { get; }

        internal abstract NullableKeyDictionary<KeyType, LinkedBucket> buckets { get; }

        protected virtual IEqualityComparer<KeyType> EqualityComparer => List.EqualityComparer;

        internal LinkedList list;
        internal LinkedBucketListNode<KeyType, ValueType>? head;
        internal int count;

        protected LinkedBucketListBase() =>
            list = null!;

        internal LinkedBucketListBase(LinkedList list) =>
            this.list = list;

        public LinkedBucketListBase(IEqualityComparer<KeyType> equalityComparer) =>
            list = new LinkedList(equalityComparer);

        internal abstract LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart GetNodePart(LinkedBucketListNode<KeyType, ValueType> node);

        internal void InsertNodeToEmptyList(LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode)
        {
            var toBeInsertedNodePart = GetNodePart(toBeInsertedNode);
            toBeInsertedNodePart.next = toBeInsertedNodePart.Owner;
            toBeInsertedNodePart.previous = toBeInsertedNodePart.Owner;
            head = toBeInsertedNodePart.Owner;
            count++;
        }

        internal void InsertNodeBefore(LinkedBucketListNode<KeyType, ValueType> node,
            LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode)
        {
            var nodePart = GetNodePart(node);
            var toBeInsertedNodePart = GetNodePart(toBeInsertedNode);
            toBeInsertedNodePart.next = nodePart.Owner;
            toBeInsertedNodePart.previous = nodePart.previous;
            nodePart.previousPart.next = toBeInsertedNodePart.Owner;
            nodePart.previous = toBeInsertedNodePart.Owner;
            count++;
        }

        internal void InsertNodeAfter(LinkedBucketListNode<KeyType, ValueType> node,
            LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode)
        {
            var afterNode = GetNodePart(node).nextPart.Owner;
            InsertNodeBefore(afterNode, toBeInsertedNode);
        }

        internal void RemoveNode(LinkedBucketListNode<KeyType, ValueType> node)
        {
            var nodePart = GetNodePart(node);

            if (nodePart.nextPart == nodePart) {
                head = null;
            } else {
                nodePart.nextPart.previous = nodePart.previous;
                nodePart.previousPart.next = nodePart.next;

                if (head == nodePart.Owner) {
                    head = nodePart.next;
                }
            }

            nodePart.Invalidate();
            count--;
        }

        internal virtual LinkedBucketListNode<KeyType, ValueType> InsertNodeFirst(LinkedBucketListNode<KeyType, ValueType> node)
        {
            if (head == null) {
                InsertNodeToEmptyList(node);
            } else {
                InsertNodeBefore(head, node);
                head = node;
            }

            return node;
        }

        internal virtual LinkedBucketListNode<KeyType, ValueType> InsertNodeLast(LinkedBucketListNode<KeyType, ValueType> node)
        {
            if (head == null) {
                InsertNodeToEmptyList(node);
            } else {
                InsertNodeBefore(head, node);
            }

            return node;
        }

        public abstract void AddBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode);
        public abstract LinkedBucketListNode<KeyType, ValueType> AddBefore(LinkedBucketListNode<KeyType, ValueType> node, ValueType value);

        public abstract void AddAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode);
        public abstract LinkedBucketListNode<KeyType, ValueType> AddAfter(LinkedBucketListNode<KeyType, ValueType> node, ValueType value);

        public abstract void AddFirst(YetNullable<KeyType> key, LinkedBucketListNode<KeyType, ValueType> node);
        public abstract LinkedBucketListNode<KeyType, ValueType> AddFirst(YetNullable<KeyType> key, ValueType value);

        public abstract void AddLast(YetNullable<KeyType> key, LinkedBucketListNode<KeyType, ValueType> node);
        public abstract LinkedBucketListNode<KeyType, ValueType> AddLast(YetNullable<KeyType> key, ValueType value);

        //public void MoveBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeMovedNode)
        //{
        //    if (ReferenceEquals(node,toBeMovedNode)) {
        //        return;
        //    }

        //    ValidateNodeToBeAttached(this, node);
        //    ValidateNodeToBeAttached(this, toBeMovedNode);

        //    RemoveNode(toBeMovedNode);
        //    InsertNodeBefore(node, toBeMovedNode);
        //}

        //public void MoveAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeMovedNode)
        //{
        //    if (ReferenceEquals(node, toBeMovedNode)) {
        //        return;
        //    }

        //    ValidateNodeToBeAttached(this, node);
        //    ValidateNodeToBeAttached(this, toBeMovedNode);

        //    toBeMovedNode.bucket.RemoveNode(toBeMovedNode);
        //    InsertNodeAfter(node, toBeMovedNode);
        //}

        //public void MoveFirst(LinkedBucketListNode<KeyType, ValueType> toBeMovedNode) {
        //    MoveBefore(head)

        //}

        public bool TryGetBucket(YetNullable<KeyType> key, out ILinkedBucketList<KeyType, ValueType> bucket)
        {
            if (buckets.TryGetValue(key, out var foundBucket)) {
                bucket = foundBucket;
                return true;
            }

            bucket = default!;
            return false;
        }

        public bool TryGetBucket(YetNullable<KeyType> key, [MaybeNullWhen(false)] out LinkedBucketListBase<KeyType, ValueType> bucket)
        {
            if (buckets.TryGetValue(key, out var foundBucket)) {
                bucket = foundBucket;
                return true;
            }

            bucket = null;
            return false;
        }

        public LinkedBucketListNode<KeyType, ValueType>? FindFirst(Predicate<ValueType> predicate)
        {
            if (predicate is null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (head != null) {
                var headPart = GetNodePart(head);
                var nodePart = headPart;

                do {
                    if (predicate(nodePart.Owner.Value)) {
                        return nodePart.Owner;
                    }

                    nodePart = nodePart.nextPart;
                } while (nodePart != headPart);
            }

            return null;
        }

        public LinkedBucketListNode<KeyType, ValueType>? FindLast(Predicate<ValueType> predicate)
        {
            if (head != null) {
                var headPart = GetNodePart(head);
                var lastPart = headPart.previousPart;
                var nodePart = lastPart;

                do {
                    if (predicate(nodePart.Owner.Value)) {
                        return nodePart.Owner;
                    }

                    nodePart = nodePart.previousPart;
                } while (nodePart != lastPart);
            }

            return null;
        }

        public LinkedBucketListNode<KeyType, ValueType>? FindFirst(ValueType value, IEqualityComparer? equalityComparer)
        {
            equalityComparer ??= EqualityComparer<ValueType>.Default;
            return FindFirst(x => equalityComparer.Equals(x, value));
        }

        public LinkedBucketListNode<KeyType, ValueType>? FindLast(ValueType value, IEqualityComparer? equalityComparer)
        {
            equalityComparer ??= EqualityComparer<ValueType>.Default;
            return FindLast(x => equalityComparer.Equals(x, value));
        }

        public virtual void Remove(LinkedBucketListNode<KeyType, ValueType> node, bool preserveEmptyBucket = false)
        {
            ValidateNodeToBeAttached(this, node);
            RemoveBucketListNode(node, preserveEmptyBucket: preserveEmptyBucket);
        }

        public void RemoveFirst(bool preserveEmptyBucket = false)
        {
            if (head == null) {
                throw new InvalidOperationException("Linked list is empty.");
            }

            RemoveBucketListNode(head, preserveEmptyBucket: preserveEmptyBucket);
        }

        public void RemoveLast(bool preserveEmptyBucket = false)
        {
            if (head == null) {
                throw new InvalidOperationException("Linked list is empty.");
            }

            var previousNode = GetNodePart(head).previousPart.Owner;
            RemoveBucketListNode(previousNode, preserveEmptyBucket: preserveEmptyBucket);
        }

        public virtual void Clear()
        {
            if (head is null) {
                return;
            }

            var nodePart = GetNodePart(head);

            do {
                var temp = nodePart;
                nodePart = nodePart.next is null ? null : GetNodePart(nodePart.next);
                temp.Invalidate();
            } while (nodePart != null);

            head = null;
            count = 0;
        }

        // TODO: Implement Enumerator class.
        public IEnumerator<ValueType> GetEnumerator()
        {
            if (head is null) {
                yield break;
            }

            var nodePart = GetNodePart(head);

            while (!(nodePart is null)) {
                yield return nodePart.Owner.Value;
                nodePart = nodePart.NextPart;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Gets item at position of <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ValueType this[int index] {
            get {
                if (head is null) {
                    throw new ArgumentOutOfRangeException();
                }

                var nodePart = GetNodePart(head);
                var currentIndex = 0;

                while (!(nodePart is null)) {
                    if (currentIndex == index) {
                        return nodePart.Owner.Value;
                    }

                    nodePart = nodePart.NextPart;
                }

                throw new ArgumentOutOfRangeException();
            }
        }

        #region IReadOnlyLinkedBucketList{`2}

        ICovariantReadOnlyNullableKeyDictionary<KeyType, IReadOnlyLinkedBucketList<KeyType, ValueType>> IReadOnlyLinkedBucketList<KeyType, ValueType>.Buckets =>
            Buckets;

        #endregion

        #region IReadOnlyLinkedBucketList{`2}

        IReadOnlyLinkedBucketListNode<KeyType, ValueType>? IReadOnlyLinkedBucketList<KeyType, ValueType>.First =>
            First;

        IReadOnlyLinkedBucketListNode<KeyType, ValueType>? IReadOnlyLinkedBucketList<KeyType, ValueType>.Last =>
            Last;

        IReadOnlyLinkedBucketList<KeyType, ValueType> IReadOnlyLinkedBucketList<KeyType, ValueType>.List =>
            List;

        IReadOnlyLinkedBucketListNode<KeyType, ValueType>? IReadOnlyLinkedBucketList<KeyType, ValueType>.FindFirst(Predicate<ValueType> predicate) => 
            FindFirst(predicate);

        IReadOnlyLinkedBucketListNode<KeyType, ValueType>? IReadOnlyLinkedBucketList<KeyType, ValueType>.FindLast(Predicate<ValueType> predicate) => 
            FindLast(predicate);

        #endregion
    }
}
