using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Extensions;

namespace Teronis.Collections.Generic
{
    public class OrderedHashSet<KeyType, ValueType> : ICollection<ValueType>
        where KeyType : notnull
        where ValueType : KeyType
    {
        public int Count =>
            valueDictionary.Count;

        public virtual bool IsReadOnly =>
            ((ICollection<ValueType>)valueDictionary).IsReadOnly;

        private readonly Dictionary<KeyType, LinkedListNode<ValueType>> valueDictionary;
        /// <summary>
        /// Only used for <see cref="GetEnumerator"/>
        /// </summary>
        private readonly LinkedList<ValueType> valueLinkedList;

        public OrderedHashSet(IEqualityComparer<KeyType> comparer)
        {
            valueDictionary = new Dictionary<KeyType, LinkedListNode<ValueType>>(comparer);
            valueLinkedList = new LinkedList<ValueType>();
        }

        public OrderedHashSet()
            : this(EqualityComparer<KeyType>.Default) { }

        public bool Add(ValueType item)
        {
            if (valueDictionary.ContainsKey(item)) {
                return false;
            }

            var node = valueLinkedList.AddLast(item);
            valueDictionary.Add(item, node);
            return true;
        }

        public bool TryGetValue(KeyType key, [MaybeNull] out ValueType value)
        {
            var hasValue = valueDictionary.TryGetValue(key, out var node);

            if (hasValue) {
                value = node!.Value;
            } else {
                value = default;
            }

            return hasValue;
        }

        void ICollection<ValueType>.Add(ValueType item)
            => Add(item);

        public bool Remove(ValueType item)
        {
            LinkedListNode<ValueType> node;

            if (!valueDictionary.TryGetValue(item, out node!)) {
                return false;
            }

            valueDictionary.Remove(item);
            valueLinkedList.Remove(node);
            return true;
        }

        public void Clear()
        {
            valueLinkedList.Clear();
            valueDictionary.Clear();
        }

        public bool Contains(ValueType item)
            => valueDictionary.ContainsKey(item);

        public IEnumerator<ValueType> GetEnumerator()
            => valueLinkedList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public IEnumerable<ValueType> YieldReversedItems()
            => valueLinkedList.YieldReversedItems();

        public void CopyTo(ValueType[] array, int arrayIndex)
            => valueLinkedList.CopyTo(array, arrayIndex);
    }

    public class OrderedHashSet<TValue> : OrderedHashSet<TValue, TValue>
        where TValue : notnull
    { }
}
