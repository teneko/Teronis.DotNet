using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    public interface ILinkedBucketList<KeyType, ValueType> : IEnumerable<ValueType>
        where KeyType : notnull
    {
        ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ILinkedBucketList<KeyType, ValueType>> Buckets { get; }
        int Count { get; }
        LinkedBucketListNode<KeyType, ValueType>? First { get; }
        bool IsBucket { get; }
        LinkedBucketListNode<KeyType, ValueType>? Last { get; }
        ILinkedBucketList<KeyType, ValueType> List { get; }

        void AddAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode);
        LinkedBucketListNode<KeyType, ValueType> AddAfter(LinkedBucketListNode<KeyType, ValueType> node, ValueType value);
        void AddBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode);
        LinkedBucketListNode<KeyType, ValueType> AddBefore(LinkedBucketListNode<KeyType, ValueType> node, ValueType value);
        void AddFirst([AllowNull] KeyType key, LinkedBucketListNode<KeyType, ValueType> node);
        LinkedBucketListNode<KeyType, ValueType> AddFirst([AllowNull] KeyType key, ValueType value);
        void AddLast([AllowNull] KeyType key, LinkedBucketListNode<KeyType, ValueType> node);
        bool TryGetBucket([AllowNull] KeyType key, [MaybeNullWhen(false)] out ILinkedBucketList<KeyType, ValueType> bucket);
        bool TryGetBucket(StillNullable<KeyType> key, [MaybeNullWhen(false)] out ILinkedBucketList<KeyType, ValueType> bucket);
        LinkedBucketListNode<KeyType, ValueType> AddLast([AllowNull] KeyType key, ValueType value);
        void Clear();
        LinkedBucketListNode<KeyType, ValueType>? FindFirst(ValueType value);
        LinkedBucketListNode<KeyType, ValueType>? FindLast(ValueType value);
        IEnumerator<ValueType> GetEnumerator();
        void Remove(LinkedBucketListNode<KeyType, ValueType> node);
        void RemoveFirst();
        void RemoveFirst(ValueType value);
        void RemoveLast();
    }
}
