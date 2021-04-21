// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Teronis.Extensions
{
    public static class IReadOnlyDictionaryGenericExtensions
    {
        /// <summary>
        /// Compares two unordered dictionaries whether they are similar.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sourceDictionary"></param>
        /// <param name="otherDictionary"></param>
        /// <param name="keyComparer"></param>
        /// <param name="valueComparer"></param>
        /// <returns>True if all key value pairs in source exists in other dictionary.</returns>
        public static bool SequenceSimilar<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue>? sourceDictionary,
            IReadOnlyDictionary<TKey, TValue>? otherDictionary,
            IEqualityComparer<TKey>? keyComparer = null,
            IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            if (ReferenceEquals(sourceDictionary, otherDictionary)) {
                return true;
            } else if (sourceDictionary is null || otherDictionary is null) {
                return false;
            }

            var valueByKeyDictionary = new Dictionary<TKey, TValue>(sourceDictionary.Count, keyComparer);

            foreach (var sourcePair in sourceDictionary) {
                valueByKeyDictionary.Add(sourcePair.Key, sourcePair.Value);
            }

            valueComparer ??= EqualityComparer<TValue>.Default;

            foreach (var otherItem in otherDictionary) {
                if (valueByKeyDictionary.TryGetValue(otherItem.Key, out var sourceItem)
                    && valueComparer.Equals(sourceItem, otherItem.Value)) {
                    valueByKeyDictionary.Remove(otherItem.Key);
                } else {
                    return false;
                }
            }

            return valueByKeyDictionary.Count == 0;
        }

        /// <summary>
        /// Compares two unordered dictionaries whether they are similar.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sourceDictionary"></param>
        /// <param name="otherDictionary"></param>
        /// <param name="keyComparer"></param>
        /// <param name="valueComparer"></param>
        /// <returns>
        /// A dictionary where each key refers to a boolean that is
        /// true if the key and value was existing in source and other
        /// dictionary.
        /// </returns>
        public static Dictionary<TKey, bool> SequenceSimilarity<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue>? sourceDictionary,
            IReadOnlyDictionary<TKey, TValue>? otherDictionary,
            IEqualityComparer<TKey>? keyComparer = null,
            IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            static Dictionary<TKey, bool> CreateDictionaryOrMapValues(IReadOnlyDictionary<TKey, TValue>? dictionary, bool defaultValue) =>
                dictionary is null
                ? new Dictionary<TKey, bool>()
                : dictionary.ToDictionary(x => x.Key, x => defaultValue);

            if (ReferenceEquals(sourceDictionary, otherDictionary)) {
                return CreateDictionaryOrMapValues(sourceDictionary, true);
            } else if (sourceDictionary is null || otherDictionary is null) {
                return sourceDictionary is null
                    ? CreateDictionaryOrMapValues(otherDictionary, false)
                    : CreateDictionaryOrMapValues(sourceDictionary, false);
            }

            var equalityByKeyDictionary = sourceDictionary.ToDictionary(
                pair => pair.Key,
                pair => false,
                keyComparer);

            valueComparer ??= EqualityComparer<TValue>.Default;

            foreach (var otherItem in otherDictionary) {
                if (sourceDictionary.TryGetValue(otherItem.Key, out var sourceItem)
                    && valueComparer.Equals(sourceItem, otherItem.Value)) {
                    equalityByKeyDictionary[otherItem.Key] = true;
                } else {
                    equalityByKeyDictionary[otherItem.Key] = false;
                }
            }

            return equalityByKeyDictionary;
        }
    }
}
