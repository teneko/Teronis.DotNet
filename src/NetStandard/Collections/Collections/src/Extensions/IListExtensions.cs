using System;
using System.Collections;
using Teronis.Tools;

namespace Teronis.Extensions
{
    public static class IListExtensions
    {
        // for ControlCollection and IList
        public static bool Swap(this IList source, int fromIndex, int toIndex, Action<int, object?> insertAt)
        {
            object? getAt(int index) => source[index];
            void removeAt(int index) => source.RemoveAt(index);
            return ListTools.SwapItem(fromIndex, toIndex, insertAt, getAt, removeAt);
        }

        // for IList
        public static bool Swap(this IList source, int fromIndex, int toIndex)
        {
            void insertAt(int index, object? item) =>
                source.Insert(index, item);

            return Swap(source, fromIndex, toIndex, insertAt);
        }
    }
}
