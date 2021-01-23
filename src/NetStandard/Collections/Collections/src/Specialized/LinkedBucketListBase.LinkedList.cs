using System.Collections.Generic;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    internal partial class LinkedBucketListBase<KeyType, ValueType>
    {
        internal class LinkedList : LinkedBucketListBase<KeyType, ValueType>
        {
            private static void insertNodeBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode)
            {
                node.bucket.List.InsertNodeBefore(node, toBeInsertedNode);
                node.bucket.InsertNodeBefore(node, toBeInsertedNode);
            }

            private static LinkedBucketListNode<KeyType, ValueType> insertNodeBefore(LinkedBucketListNode<KeyType, ValueType> node, ValueType value)
            {
                var toBeInsertedNode = new LinkedBucketListNode<KeyType, ValueType>(value);
                insertNodeBefore(node, toBeInsertedNode);
                return node;
            }

            private static void insertBucketListNodeAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode)
            {
                node.bucket.List.InsertNodeAfter(node, toBeInsertedNode);
                node.bucket.InsertNodeAfter(node, toBeInsertedNode);
            }

            private static LinkedBucketListNode<KeyType, ValueType> insertBucketListNodeAfter(LinkedBucketListNode<KeyType, ValueType> node, ValueType value)
            {
                var toBeInsertedNode = new LinkedBucketListNode<KeyType, ValueType>(value);
                insertBucketListNodeAfter(node, toBeInsertedNode);
                return node;
            }

            private static void insertBucketListNodeFirst(LinkedBucket bucket, LinkedBucketListNode<KeyType, ValueType> node)
            {
                bucket.List.InsertNodeFirst(node);
                bucket.InsertNodeFirst(node);
            }

            private static LinkedBucketListNode<KeyType, ValueType> insertBucketListNodeFirst(LinkedBucket bucket, ValueType value)
            {
                var node = new LinkedBucketListNode<KeyType, ValueType>(value);
                insertBucketListNodeFirst(bucket, node);
                return node;
            }

            private static void insertBucketListNodeLast(LinkedBucket bucket, LinkedBucketListNode<KeyType, ValueType> node)
            {
                bucket.List.InsertNodeLast(node);
                bucket.InsertNodeLast(node);
            }

            private static LinkedBucketListNode<KeyType, ValueType> insertBucketListNodeLast(LinkedBucket bucket, ValueType value)
            {
                var node = new LinkedBucketListNode<KeyType, ValueType>(value);
                insertBucketListNodeLast(bucket, node);
                return node;
            }

            public override bool IsBucket => false;

            internal override NullableKeyDictionary<KeyType, LinkedBucket> buckets { get; }

            protected override IEqualityComparer<KeyType> EqualityComparer { get; }

            public LinkedList(IEqualityComparer<KeyType> equalityComparer)
            {
                list = this;
                buckets = new NullableKeyDictionary<KeyType, LinkedBucket>(equalityComparer);
                EqualityComparer = equalityComparer;
            }

            internal override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart GetNodePart(LinkedBucketListNode<KeyType, ValueType> node) =>
                node.ListPart;

            public override void AddBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> newNode)
            {
                ValidateNodeToBeAttached(this, newNode);
                ValidateNodeToBeNew(newNode);
                insertNodeBefore(node, newNode);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddBefore(LinkedBucketListNode<KeyType, ValueType> node, ValueType value)
            {
                ValidateNodeToBeAttached(this, node);
                return insertNodeBefore(node, value);
            }

            public override void AddAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> newNode)
            {
                ValidateNodeToBeAttached(this, node);
                ValidateNodeToBeNew(newNode);
                insertBucketListNodeAfter(node, newNode);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddAfter(LinkedBucketListNode<KeyType, ValueType> node, ValueType value)
            {
                ValidateNodeToBeAttached(this, node);
                var addedNode = insertBucketListNodeAfter(node, value);
                return addedNode;
            }

            public override void AddFirst(YetNullable<KeyType> key, LinkedBucketListNode<KeyType, ValueType> newNode)
            {
                ValidateNodeToBeNew(newNode);

                if (!buckets.TryGetValue(key, out var bucket)) {
                    bucket = new LinkedBucket(this, key);
                    buckets.Add(key, bucket);
                }

                insertBucketListNodeFirst(bucket, newNode);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddFirst(YetNullable<KeyType> key, ValueType value)
            {
                if (!buckets.TryGetValue(key, out var bucket)) {
                    bucket = new LinkedBucket(this, key);
                    buckets.Add(key, bucket);
                }

                return insertBucketListNodeFirst(bucket, value);
            }

            public override void AddLast(YetNullable<KeyType> key, LinkedBucketListNode<KeyType, ValueType> newNode)
            {
                ValidateNodeToBeNew(newNode);

                if (!buckets.TryGetValue(key, out var bucket)) {
                    bucket = new LinkedBucket(this, key);
                    buckets.Add(key, bucket);
                }

                insertBucketListNodeLast(bucket, newNode);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddLast(YetNullable<KeyType> key, ValueType value)
            {
                if (!buckets.TryGetValue(key, out var bucket)) {
                    bucket = new LinkedBucket(this, key);
                    buckets.Add(key, bucket);
                }

                return insertBucketListNodeLast(bucket, value);
            }

            public override void Clear()
            {
                if (head is null) {
                    return;
                }

                foreach (var bucket in buckets.Values) {
                    bucket.Clear(true);
                    bucket.Invalidate();
                }

                buckets.Clear();
                head = null;
                count = 0;
            }
        }
    }
}
