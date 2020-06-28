using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Specialized
{
    public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IOrderedDictionary, IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        new KeyValuePair<TKey, TValue> this[int index] { get; set; }
        [MaybeNull,AllowNull]
        new TValue this[TKey key] { get; set; }
        new int Count { get; }
        new ICollection<TKey> Keys { get; }
        new ICollection<TValue> Values { get; }
        new void Add(TKey key, [AllowNull] TValue value);
        new void Clear();
        void Insert(int index, TKey key, TValue value);
        int IndexOf(TKey key);
        bool ContainsValue([AllowNull]TValue value);
        bool ContainsValue([AllowNull] TValue value, IEqualityComparer<TValue> comparer);
        new bool ContainsKey(TKey key);
        new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
        new bool Remove(TKey key);
        new void RemoveAt(int index);
        new bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);
        [return: MaybeNull]
        TValue GetValue(TKey key);
        void SetValue(TKey key, [AllowNull] TValue value);
        KeyValuePair<TKey, TValue> GetItem(int index);
        void SetItem(int index, [AllowNull] TValue value);
    }
}
