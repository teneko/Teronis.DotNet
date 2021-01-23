using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class YieldIteratorInfluencedReadOnlyListExtensions
    {
        public static IList<ItemType> AsIList<ItemType>(this IList<ItemType> list) =>
            list;

        public static YieldIteratorInfluencedReadOnlyList<ItemType> ToYieldIteratorInfluencedReadOnlyList<ItemType>(this IList<ItemType> list) =>
            new YieldIteratorInfluencedReadOnlyList<ItemType>.List(list);

        public static IReadOnlyList<ItemType> AsIReadOnlyList<ItemType>(this IReadOnlyList<ItemType> list) =>
            list;

        public static YieldIteratorInfluencedReadOnlyList<ItemType> ToYieldIteratorInfluencedReadOnlyList<ItemType>(this IReadOnlyList<ItemType> list) =>
            new YieldIteratorInfluencedReadOnlyList<ItemType>.ReadOnlyList(list);
    }
}
