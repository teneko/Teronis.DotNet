using System.Collections.Generic;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class IListGenericExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = ThreadSafeRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Move<T>(this IList<T> list, T from, T to)
        {
            var fromIndex = list.IndexOf(from);
            var toIndex = list.IndexOf(to);
            Move(list, fromIndex, toIndex);
        }

        public static void Move<T>(this IList<T> list, int fromIndex, int toIndex)
            => ListTools.MoveItem<IList<T>, T>(fromIndex, toIndex, (index) => list[index], (index, item) => list.Insert(index, item), (index) => list.RemoveAt(index));
    }
}
