using System;
using System.Collections;

namespace Teronis.NetStandard.Extensions
{
    public static class IListExtensions
    {
        // for ControlCollection and IList
        public static bool Swap(this IList source, int fromIndex, int toIndex, Action<int, object> insertAt)
        {
            Func<int, object> getAt = (index) => source[index];
            Action<int> removeAt = (index) => source.RemoveAt(index);
            return Tools.SwapTools.Swap(fromIndex, toIndex, insertAt, getAt, removeAt);
        }

        // for IList
        public static bool Swap(this IList source, int fromIndex, int toIndex)
        {
            Action<int, object> insertAt = (index, item) => source.Insert(index, item);
            return Swap(source, fromIndex, toIndex, insertAt);
        }
    }
}
