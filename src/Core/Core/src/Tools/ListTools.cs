using System;
using System.Collections.Generic;

namespace Teronis.Tools
{
    public class ListTools
    {
        /// <summary>
        /// A generic swap-item version for list, collection or dictionary.
        /// </summary>
        public static bool SwapItem(int fromIndex, int toIndex, Action<int, object> insertAt, Func<int, object> getAt, Action<int> removeAt)
        {
            Action<int, int> swap = (smallerIndex, biggerIndex) => {
                var smallerItem = getAt(smallerIndex);
                var biggerItem = getAt(biggerIndex);

                removeAt(biggerIndex);
                removeAt(smallerIndex);
                insertAt(smallerIndex, biggerItem);
                insertAt(biggerIndex, smallerItem);
            };

            if (fromIndex == toIndex)
                return false;
            else {
                var sortedList = new List<int> { fromIndex, toIndex };
                sortedList.Sort();
                swap(sortedList[0], sortedList[1]);
                return true;
            }
        }

        /// <summary>
        /// A generic move-item version for list, collection or dictionary.
        /// </summary>
        public static void MoveItem<S, T>(int fromIndex, int toIndex, Func<int, T> getItemAt, Action<int, T> insertItem, Action<int> removeItemAt)
        {
            var from = getItemAt(fromIndex);
            //removeItemAt(toIndex < fromIndex ? fromIndex + 1 : fromIndex);
            //insertItem(fromIndex < toIndex ? toIndex + 1 : toIndex, from);
            removeItemAt(fromIndex);
            insertItem(toIndex, from);
        }
    }
}
