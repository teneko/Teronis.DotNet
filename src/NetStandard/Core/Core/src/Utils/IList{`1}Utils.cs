// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Threading;

namespace Teronis.Utils
{
    public class IListGenericUtils
    {
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;

            while (n > 1) {
                n--;
                int k = ThreadSafeRandom.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Bubblesort<T>(IList<T> list, IComparer<T> comparer)
        {
            // When null, throw exception
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }

            // There have to be at least two items to sort them
            if (list.Count < 2) {
                return;
            }

            // -1 for zero based index and another 
            // -1 for beginning at the second last index
            var currentIndex = list.Count - 2;

            while (currentIndex >= 0) {
                for (var index = 0; index <= currentIndex; index++) {
                    var foreNumber = list[index];
                    var backIndex = index + 1;
                    var backNumber = list[backIndex];

                    if (comparer.Compare(foreNumber, backNumber) > 0) {
                        list[backIndex] = foreNumber;
                        list[index] = backNumber;
                    }
                }

                currentIndex--;
            }
        }

        public static void Bubblesort<T>(IList<T> list)
            => Bubblesort(list, Comparer<T>.Default);

        public static int BinarySearch<T>(IList<T> items, T item, IComparer<T> comparer)
        {
            int minIndex = 0;
            int maxIndex = items.Count - 1;

            while (minIndex <= maxIndex) {
                int mid = (minIndex + maxIndex) / 2;
                var compare = comparer.Compare(item, items[mid]);

                if (compare == 0) {
                    return ++mid;
                } else if (compare < 0) {
                    maxIndex = mid - 1;
                } else {
                    minIndex = mid + 1;
                }
            }

            return -1;
        }

        public static int BinarySearch<T>(IList<T> list, T item) =>
            BinarySearch(list, item, Comparer<T>.Default);

        public static IEnumerable<T> YieldReverse<T>(IList<T> list, int index, int count)
        {
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index), "The index is smaller than zero.");
            }

            var lastIndex = index + count - 1;

            if (lastIndex >= list.Count) {
                throw new ArgumentOutOfRangeException(nameof(index), "The sum of index and count exceeds the border of list.");
            }

            for (var currentIndex = lastIndex; currentIndex >= index; currentIndex--) {
                yield return list[currentIndex];
            }
        }

        public static IEnumerable<(int Index, T Item)> YieldIndexedReverse<T>(IList<T> list, int index, int count)
        {
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index), "The index is smaller than zero.");
            }

            var lastIndex = index + count - 1;

            if (lastIndex >= list.Count) {
                throw new ArgumentOutOfRangeException(nameof(index), "The sum of index and count exceeds the border of list.");
            }

            for (var currentIndex = lastIndex; currentIndex >= index; currentIndex--) {
                yield return (Index: currentIndex, Item: list[currentIndex]);
            }
        }
    }
}
