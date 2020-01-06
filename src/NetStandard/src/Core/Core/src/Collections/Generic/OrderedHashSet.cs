using System.Collections;
using System.Collections.Generic;
using Teronis.Extensions;

namespace Teronis.Collections.Generic
{
    public class OrderedHashSet<TKey, TValue> : ICollection<TValue>
        where TValue : TKey
    {
        public int Count => valueDictionary.Count;
        public virtual bool IsReadOnly => ((ICollection<TValue>)valueDictionary).IsReadOnly;

        private readonly Dictionary<TKey, LinkedListNode<TValue>> valueDictionary;
        /// <summary>
        /// Only used for <see cref="GetEnumerator"/>
        /// </summary>
        private readonly LinkedList<TValue> valueLinkedList;

        public OrderedHashSet(IEqualityComparer<TKey> comparer)
        {
            valueDictionary = new Dictionary<TKey, LinkedListNode<TValue>>(comparer);
            valueLinkedList = new LinkedList<TValue>();
        }

        public OrderedHashSet()
            : this(EqualityComparer<TKey>.Default) { }

        public bool Add(TValue item)
        {
            if (valueDictionary.ContainsKey(item))
                return false;

            var node = valueLinkedList.AddLast(item);
            valueDictionary.Add(item, node);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var hasValue = valueDictionary.TryGetValue(key, out var node);

            if (hasValue)
                value = node.Value;
            else
                value = default;

            return hasValue;
        }

        void ICollection<TValue>.Add(TValue item)
            => Add(item);

        public bool Remove(TValue item)
        {
            LinkedListNode<TValue> node;

            if (!valueDictionary.TryGetValue(item, out node))
                return false;

            valueDictionary.Remove(item);
            valueLinkedList.Remove(node);
            return true;
        }

        public void Clear()
        {
            valueLinkedList.Clear();
            valueDictionary.Clear();
        }

        public bool Contains(TValue item)
            => valueDictionary.ContainsKey(item);

        public IEnumerator<TValue> GetEnumerator()
            => valueLinkedList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public IEnumerable<TValue> YieldReversedItems()
            => valueLinkedList.YieldReversedItems();

        public void CopyTo(TValue[] array, int arrayIndex)
            => valueLinkedList.CopyTo(array, arrayIndex);
    }

    public class OrderedHashSet<TValue> : OrderedHashSet<TValue, TValue> { }
}
