using System.Collections.Generic;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class IListGenericExtensions
    {
        public static void Shuffle<T>(this IList<T> list) =>
            IListGenericUtils.Shuffle(list);

        public static void Move<T>(this IList<T> list, int fromIndex, int toIndex) =>
            ListUtils.MoveItem(
                fromIndex,
                toIndex,
                (index) => list[index],
                (index) => list.RemoveAt(index),
                (index, item) => list.Insert(index, item));

        public static void Move<T>(this IList<T> list, T from, T to)
        {
            var fromIndex = list.IndexOf(from);
            var toIndex = list.IndexOf(to);
            Move(list, fromIndex, toIndex);
        }

        public static void Move<T>(this IList<T> list, int fromIndex, int toIndex, int count) =>
            ListUtils.MoveItems(
                fromIndex,
                toIndex,
                count,
                index => list[index],
                list.RemoveAt,
                list.Insert);
    }
}
