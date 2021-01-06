using System;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    internal abstract partial class LinkedBucketListBase<KeyType, ValueType> where KeyType : notnull
    {
        internal class LinkedBucket : LinkedBucketListBase<KeyType, ValueType>
        {
            public override bool IsBucket => true;

            internal override NullableKeyDictionary<KeyType, LinkedBucket> buckets => List.buckets;

            public LinkedBucket(LinkedList list)
                : base(list) { }

            internal override LinkedBucketListNode<KeyType, ValueType>.LinkedBucketListNodePart GetNodePart(LinkedBucketListNode<KeyType, ValueType> node) =>
                node.BucketPart;

            internal override LinkedBucketListNode<KeyType, ValueType> InsertNodeFirst(LinkedBucketListNode<KeyType, ValueType> node)
            {
                base.InsertNodeFirst(node);
                node.bucket = this;
                return node;
            }

            internal override LinkedBucketListNode<KeyType, ValueType> InsertNodeLast(LinkedBucketListNode<KeyType, ValueType> node)
            {
                base.InsertNodeLast(node);
                node.bucket = this;
                return node;
            }

            private void validateBucket()
            {
                if (list is null) {
                    throw new InvalidOperationException("This bucket is not attached.");
                }
            }

            public override void AddBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> newNode)
            {
                validateBucket();
                list.AddBefore(node, newNode);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddBefore(LinkedBucketListNode<KeyType, ValueType> node, ValueType value)
            {
                validateBucket();
                return list.AddBefore(node, value);
            }

            public override void AddAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> newNode)
            {
                validateBucket();
                list.AddAfter(node, newNode);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddAfter(LinkedBucketListNode<KeyType, ValueType> node, ValueType value)
            {
                validateBucket();
                return list.AddAfter(node, value);
            }

            public override void AddFirst([AllowNull] KeyType key, LinkedBucketListNode<KeyType, ValueType> node)
            {
                validateBucket();
                list.AddFirst(key, node);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddFirst([AllowNull] KeyType key, ValueType value)
            {
                validateBucket();
                return list.AddFirst(key, value);
            }

            public override void AddLast([AllowNull] KeyType key, LinkedBucketListNode<KeyType, ValueType> node)
            {
                validateBucket();
                list.AddLast(key, node);
            }

            public override LinkedBucketListNode<KeyType, ValueType> AddLast([AllowNull] KeyType key, ValueType value)
            {
                validateBucket();
                return list.AddLast(key, value);
            }

            internal void Clear(bool invokedFromList)
            {
                if (invokedFromList) {
                    base.Clear();
                } else {
                    if (head is null) {
                        return;
                    }

                    var node = head;

                    do {
                        var temp = node;
                        node = node.BucketPart.Next;
                        Remove(temp);
                    } while (node != null);

                    head = null;
                    count = 0;
        }
            }

            public override void Clear()
            {
                validateBucket();
                Clear(false);
            }
        }
    }
}
