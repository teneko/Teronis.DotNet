using System;
using System.Collections.Generic;
using System.Text;
using Teronis.NetStandard.Extensions;

namespace Teronis.NetStandard.Tools
{
    public static class SwapTools
    {
        // for each kind of list, collection or dictionary
        public static bool Swap(int fromIndex, int toIndex, Action<int, object> insertAt, Func<int, object> getAt, Action<int> removeAt)
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
                var sortedList = new List<int> { fromIndex, toIndex }.SortAndReturn();
                swap(sortedList[0], sortedList[1]);
                return true;
            }
        }
    }
}
