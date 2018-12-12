using System;
using System.Collections.Generic;
using System.Text;
using Teronis.NetStandard.Collections.Generic;

namespace Teronis.NetStandard.Extensions
{
    public static class OrderedDictionaryExtensions
    {
        public static bool Swap<K, V>(this OrderedDictionary<K, V> source, int fromIndex, int toIndex)
        {
            Action<int, object> insertAt = (index, item) => source.Insert(index, (KeyValuePair<K, V>)item);
            Func<int, object> getAt = (index) => source[index];
            Action<int> removeAt = (index) => source.RemoveAt(index);
            return Tools.SwapTools.Swap(fromIndex, toIndex, insertAt, getAt, removeAt);
        }
    }
}
