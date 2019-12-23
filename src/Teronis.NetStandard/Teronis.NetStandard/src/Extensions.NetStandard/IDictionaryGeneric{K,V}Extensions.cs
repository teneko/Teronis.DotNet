using System;
using System.Collections.Generic;

namespace Teronis.Extensions.NetStandard
{
    public static class IDictionaryGenericExtensions
    {
        public static V AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (!dictionary.ContainsKey(key))
                return dictionary.AddAndReturn(key, value);
            else
                return dictionary[key] = value;
        }

        public static V AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value, Func<V, V> getUpdatedValue)
        {
            if (!dictionary.ContainsKey(key))
                return dictionary.AddAndReturn(key, value);
            else if (getUpdatedValue != null)
                return dictionary[key] = getUpdatedValue(dictionary[key]);
            else
                return dictionary[key];
        }

        /// <summary>
        /// Does not throw an exception when key does not exist, instead the default value will be returned.
        /// </summary>
        public static V GetValue<K, V>(this IDictionary<K, V> dictionary, K key) => dictionary.TryGetValue(key, out V value) ? value : value;

        public static V AddAndReturn<K, V>(this IDictionary<K, V> source, K key, V value)
        {
            source.Add(key, value);
            return value;
        }

        public static V RemoveAndReturn<K, V>(this IDictionary<K, V> source, K key)
        {
            var value = source[key];
            source.Remove(key);
            return value;
        }

        public static bool TryRemove<K, V>(this IDictionary<K, V> source, K key, out V value)
        {
            if (source.TryGetValue(key, out value)) {
                source.Remove(key);
                return true;
            } else
                return false;
        }
    }
}