using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    public interface ILinkedBucketList<KeyType, ValueType> : IReadOnlyLinkedBucketList<KeyType, ValueType>, IReadOnlyList<ValueType>, IEnumerable<ValueType>
        where KeyType : notnull
    {
        new ICovariantReadOnlyNullableKeyDictionary<KeyType, ILinkedBucketList<KeyType, ValueType>> Buckets { get; }
        new int Count { get; }
        new LinkedBucketListNode<KeyType, ValueType>? First { get; }
        new bool IsBucket { get; }
        new LinkedBucketListNode<KeyType, ValueType>? Last { get; }
        new ILinkedBucketList<KeyType, ValueType> List { get; }

        LinkedBucketListNode<KeyType, ValueType> AddAfter(LinkedBucketListNode<KeyType, ValueType> node, ValueType value);
        LinkedBucketListNode<KeyType, ValueType> AddBefore(LinkedBucketListNode<KeyType, ValueType> node, ValueType value);
        LinkedBucketListNode<KeyType, ValueType> AddFirst(YetNullable<KeyType> key, ValueType value);
        LinkedBucketListNode<KeyType, ValueType> AddLast(YetNullable<KeyType> key, ValueType value);

        void AddAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode);
        void AddBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode);
        void AddFirst(YetNullable<KeyType> key, LinkedBucketListNode<KeyType, ValueType> node);
        void AddLast(YetNullable<KeyType> key, LinkedBucketListNode<KeyType, ValueType> node);

        void Remove(LinkedBucketListNode<KeyType, ValueType> node, bool preserveEmptyBucket = false);
        void RemoveFirst(bool preserveEmptyBucket = false);
        void RemoveLast(bool preserveEmptyBucket = false);

        void Clear();

        bool TryGetBucket(YetNullable<KeyType> key, [MaybeNullWhen(false)] out ILinkedBucketList<KeyType, ValueType> bucket);

        new LinkedBucketListNode<KeyType, ValueType>? FindFirst(Predicate<ValueType> predicate);
        new LinkedBucketListNode<KeyType, ValueType>? FindLast(Predicate<ValueType> predicate);

        new IEnumerator<ValueType> GetEnumerator();
    }
}
