// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teronis.Extensions
{
    public static partial class IEnumerableGenericExtensions
    {
        /// <summary>
        /// Adds the item at the end of a new <see cref="IEnumerable{T}"/> sequence without loosing the target items.
        /// </summary>
        public static IEnumerable<T> ContinueWith<T>(this IEnumerable<T> target, T item)
        {
            if (target == null) {
                throw new ArgumentException(nameof(target));
            }

            foreach (var t in target) {
                yield return t;
            }

            yield return item;
        }

        /// <summary>
        /// Adds the items at the end of a new <see cref="IEnumerable{T}"/> sequence without loosing the target items.
        /// </summary>
        public static IEnumerable<T> ContinueWith<T>(this IEnumerable<T> target, IEnumerable<T> items)
        {
            if (target == null) {
                throw new ArgumentException(nameof(target));
            }

            foreach (var t in target) {
                yield return t;
            }

            foreach (var t in items) {
                yield return t;
            }
        }

        /// <summary>
        /// Compares two unordered enumerables whether they are similar. Supports duplicates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceEnumerable"></param>
        /// <param name="otherEnumerable"></param>
        /// <param name="comparer"></param>
        /// <returns>True if all items in source exists in other enumerable.</returns>
        public static bool SequenceSimilar<T>(
            this IEnumerable<T>? sourceEnumerable,
            IEnumerable<T>? otherEnumerable,
            IEqualityComparer<T>? comparer = null)
            where T : notnull
        {
            if (ReferenceEquals(sourceEnumerable, otherEnumerable)) {
                return true;
            } else if (sourceEnumerable is null || otherEnumerable is null) {
                return false;
            }

            var balanceValueByKeyDictionary = new Dictionary<T, int>(comparer);

            foreach (var item in sourceEnumerable) {
                if (balanceValueByKeyDictionary.ContainsKey(item)) {
                    balanceValueByKeyDictionary[item]++;
                } else {
                    balanceValueByKeyDictionary.Add(item, 1);
                }
            }

            foreach (var item in otherEnumerable) {
                if (balanceValueByKeyDictionary.ContainsKey(item)) {
                    balanceValueByKeyDictionary[item]--;
                } else {
                    return false;
                }
            }

            return balanceValueByKeyDictionary.Values.All(c => c == 0);
        }

        /// <summary>
        /// Compares two unordered enumerables whether they are similar. Supports duplicates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceEnumerable"></param>
        /// <param name="otherEnumerable"></param>
        /// <param name="comparer"></param>
        /// <returns>
        /// A dictionary where each key refers to a boolean that is
        /// true if the key was existing in source and other enumerable.
        /// </returns>
        public static Dictionary<T, bool> SequenceSimilarity<T>(
            this IEnumerable<T>? sourceEnumerable,
            IEnumerable<T>? otherEnumerable,
            IEqualityComparer<T>? comparer = null)
            where T : notnull
        {
            static Dictionary<T, bool> CreateDictionaryOrMapValues(IEnumerable<T>? enumerable, bool defaultValue) =>
                enumerable is null
                ? new Dictionary<T, bool>()
                : enumerable.ToDictionary(x => x, x => defaultValue);

            if (ReferenceEquals(sourceEnumerable, otherEnumerable)) {
                return CreateDictionaryOrMapValues(sourceEnumerable, defaultValue: true);
            } else if (sourceEnumerable is null || otherEnumerable is null) {
                return sourceEnumerable is null
                    ? CreateDictionaryOrMapValues(otherEnumerable, defaultValue: false)
                    : CreateDictionaryOrMapValues(sourceEnumerable, defaultValue: false);
            }

            var balanceValueByKeyDictionary = new Dictionary<T, int>(comparer);

            foreach (var item in sourceEnumerable) {
                if (balanceValueByKeyDictionary.ContainsKey(item)) {
                    balanceValueByKeyDictionary[item]++;
                } else {
                    balanceValueByKeyDictionary.Add(item, 1);
                }
            }

            foreach (var item in otherEnumerable) {
                if (balanceValueByKeyDictionary.ContainsKey(item)) {
                    balanceValueByKeyDictionary[item]--;
                } else {
                    // Add the key with negative number to guarantee falsy 
                    // equality. Having the key to ensures collection union.
                    balanceValueByKeyDictionary.Add(item, -1);
                }
            }

            return balanceValueByKeyDictionary.ToDictionary(x => x.Key, x => {
                // True means similar.
                return x.Value == 0;
            });
        }
    }
}
