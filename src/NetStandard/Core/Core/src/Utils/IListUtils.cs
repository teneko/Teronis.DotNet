using System;
using System.Collections.Generic;
using Teronis.Threading;

namespace Teronis.Utils
{
    public class IListUtils
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

        public static void Bubblesort<T>(IList<T> collection, IComparer<T> comparer)
        {
            // When null, throw exception
            if (collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }

            // There have to be at least two items to sort them
            if (collection.Count < 2) {
                return;
            }

            // -1 for zero based index and another 
            // -1 for beginning at the second last index
            var currentIndex = collection.Count - 2;

            while (currentIndex >= 0) {
                for (var index = 0; index <= currentIndex; index++) {
                    var foreNumber = collection[index];
                    var backIndex = index + 1;
                    var backNumber = collection[backIndex];

                    if (comparer.Compare(foreNumber, backNumber) > 0) {
                        collection[backIndex] = foreNumber;
                        collection[index] = backNumber;
                    }
                }

                currentIndex--;
            }
        }

        public static void Bubblesort<T>(IList<T> collection)
            => Bubblesort(collection, Comparer<T>.Default);
    }
}
