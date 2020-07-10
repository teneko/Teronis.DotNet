using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Extensions
{
    public static class IDictionaryGenericExtensions
    {
        public static V AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value)
            where K : notnull
        {
            if (!dictionary.ContainsKey(key)) {
                return dictionary.AddAndReturn(key, value);
            } else {
                return dictionary[key] = value;
            }
        }

        public static V AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value, Func<V, V> repalceValue)
            where K : notnull
        {
            repalceValue = repalceValue ?? throw new ArgumentNullException(nameof(repalceValue));

            if (dictionary.TryGetValue(key, out var dictionaryValue)) {

                if (repalceValue != null) {
                    return dictionary[key] = repalceValue(dictionaryValue);
                }

                return dictionaryValue;
            }

            return dictionary.AddAndReturn(key, value);
        }

        /// <summary>
        /// Does not throw an exception when key does not exist, instead the default value will be returned.
        /// </summary>
        [return: MaybeNull]
        public static V GetValue<K, V>(this IDictionary<K, V> dictionary, K key)
            where K : notnull =>
            dictionary.TryGetValue(key, out V value) ? value : value;

        public static V AddAndReturn<K, V>(this IDictionary<K, V> source, K key, V value)
            where K : notnull
        {
            source.Add(key, value);
            return value;
        }

        public static V RemoveAndReturn<K, V>(this IDictionary<K, V> source, K key)
            where K : notnull
        {
            var value = source[key];
            source.Remove(key);
            return value;
        }

        public static bool TryRemove<K, V>(this IDictionary<K, V> source, K key, [MaybeNullWhen(false)] out V value)
            where K : notnull
        {
            if (source.TryGetValue(key, out value)) {
                source.Remove(key);
                return true;
            } else {
                return false;
            }
        }
    }
}
