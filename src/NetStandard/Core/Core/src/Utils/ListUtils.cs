// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Utils
{
    public class ListUtils
    {
        /// <summary>
        /// A generic swap-item version for list, collection or dictionary.
        /// </summary>
        public static bool SwapItem(int fromIndex, int toIndex, Action<int, object?> insertAt, Func<int, object?> getAt, Action<int> removeAt)
        {
            void swap(int smallerIndex, int biggerIndex)
            {
                var smallerItem = getAt(smallerIndex);
                var biggerItem = getAt(biggerIndex);

                removeAt(biggerIndex);
                removeAt(smallerIndex);
                insertAt(smallerIndex, biggerItem);
                insertAt(biggerIndex, smallerItem);
            }

            if (fromIndex == toIndex) {
                return false;
            } else {
                var sortedList = new List<int> { fromIndex, toIndex };
                sortedList.Sort();
                swap(sortedList[0], sortedList[1]);
                return true;
            }
        }

        /// <summary>
        /// A generic move-item version for list, collection or dictionary.
        /// </summary>
        public static void MoveItem<T>(int fromIndex, int toIndex, Func<int, T> getItemAt, Action<int> removeItemAt, Action<int, T> insertItem)
        {
            var from = getItemAt(fromIndex);
            removeItemAt(fromIndex);
            insertItem(toIndex, from);
        }

        private static void insertItems<T>(int toIndex, T[] items, Action<int, T> insertItem)
        {
            var itemsLength = items.Length;

            for (var index = 0; index < itemsLength; index++) {
                insertItem(toIndex + index, items[index]);
            }
        }

        /// <summary>
        /// A generic move-item version for list, collection or dictionary.
        /// </summary>
        public static void MoveItems<T>(int fromIndex, int toIndex, int count, Func<int, T> getItemAt, Action<int> removeItemAt, Action<int, T> insertItem)
        {
            var items = new T[count];

            for (var index = count - 1; index >= 0; index--) {
                var nextIndex = fromIndex + index;
                items[index] = getItemAt(nextIndex);
                removeItemAt(nextIndex);
            }

            insertItems(toIndex, items, insertItem);
        }
    }
}
