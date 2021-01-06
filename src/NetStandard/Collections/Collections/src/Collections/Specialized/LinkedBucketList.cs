using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Specialized
{
    public class LinkedBucketList<KeyType, ValueType> : ILinkedBucketList<KeyType, ValueType>
        where KeyType : notnull
    {
        private LinkedBucketListBase<KeyType, ValueType>.LinkedList linkedList;

        public LinkedBucketList(IEqualityComparer<KeyType> equalityComparer) =>
            linkedList = new LinkedBucketListBase<KeyType, ValueType>.LinkedList(equalityComparer);

        public ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ILinkedBucketList<KeyType, ValueType>> Buckets => ((ILinkedBucketList<KeyType, ValueType>)linkedList).Buckets;
        public int Count => ((ILinkedBucketList<KeyType, ValueType>)linkedList).Count;
        public LinkedBucketListNode<KeyType, ValueType>? First => ((ILinkedBucketList<KeyType, ValueType>)linkedList).First;
        public bool IsBucket => ((ILinkedBucketList<KeyType, ValueType>)linkedList).IsBucket;
        public LinkedBucketListNode<KeyType, ValueType>? Last => ((ILinkedBucketList<KeyType, ValueType>)linkedList).Last;
        public ILinkedBucketList<KeyType, ValueType> List => ((ILinkedBucketList<KeyType, ValueType>)linkedList).List;

        public void AddAfter(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddAfter(node, toBeInsertedNode);

        public LinkedBucketListNode<KeyType, ValueType> AddAfter(LinkedBucketListNode<KeyType, ValueType> node, ValueType value) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddAfter(node, value);

        public void AddBefore(LinkedBucketListNode<KeyType, ValueType> node, LinkedBucketListNode<KeyType, ValueType> toBeInsertedNode) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddBefore(node, toBeInsertedNode);

        public LinkedBucketListNode<KeyType, ValueType> AddBefore(LinkedBucketListNode<KeyType, ValueType> node, ValueType value) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddBefore(node, value);

        public void AddFirst([AllowNull] KeyType key, LinkedBucketListNode<KeyType, ValueType> node) =>
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddFirst(key, node);

        public LinkedBucketListNode<KeyType, ValueType> AddFirst([AllowNull] KeyType key, ValueType value) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddFirst(key, value);

        public void AddLast([AllowNull] KeyType key, LinkedBucketListNode<KeyType, ValueType> node) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddLast(key, node);

        public LinkedBucketListNode<KeyType, ValueType> AddLast([AllowNull] KeyType key, ValueType value) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).AddLast(key, value);

        public void Clear() => ((ILinkedBucketList<KeyType, ValueType>)linkedList).Clear();
        public LinkedBucketListNode<KeyType, ValueType>? FindFirst(ValueType value) => ((ILinkedBucketList<KeyType, ValueType>)linkedList).FindFirst(value);
        public LinkedBucketListNode<KeyType, ValueType>? FindLast(ValueType value) => ((ILinkedBucketList<KeyType, ValueType>)linkedList).FindLast(value);
        public IEnumerator<ValueType> GetEnumerator() => ((ILinkedBucketList<KeyType, ValueType>)linkedList).GetEnumerator();
        public void Remove(LinkedBucketListNode<KeyType, ValueType> node) => ((ILinkedBucketList<KeyType, ValueType>)linkedList).Remove(node);
        public void RemoveFirst() => ((ILinkedBucketList<KeyType, ValueType>)linkedList).RemoveFirst();
        public void RemoveFirst(ValueType value) => ((ILinkedBucketList<KeyType, ValueType>)linkedList).RemoveFirst(value);
        public void RemoveLast() => ((ILinkedBucketList<KeyType, ValueType>)linkedList).RemoveLast();

        public bool TryGetBucket([AllowNull] KeyType key, [MaybeNullWhen(false)] out ILinkedBucketList<KeyType, ValueType> bucket) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).TryGetBucket(key, out bucket);

        public bool TryGetBucket(StillNullable<KeyType> key, [MaybeNullWhen(false)] out ILinkedBucketList<KeyType, ValueType> bucket) => 
            ((ILinkedBucketList<KeyType, ValueType>)linkedList).TryGetBucket(key, out bucket);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)linkedList).GetEnumerator();
    }
}
