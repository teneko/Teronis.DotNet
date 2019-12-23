using System;
using System.Collections.Generic;
using Teronis.Collections.Generic;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class OrderedDictionaryExtensions
    {
        public static bool Swap<K, V>(this OrderedDictionary<K, V> source, int fromIndex, int toIndex)
        {
            Action<int, object> insertAt = (index, item) => source.Insert(index, (KeyValuePair<K, V>)item);
            Func<int, object> getAt = (index) => source[index];
            Action<int> removeAt = (index) => source.RemoveAt(index);
            return ListTools.SwapItem(fromIndex, toIndex, insertAt, getAt, removeAt);
        }
    }
}
