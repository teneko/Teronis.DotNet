// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    internal static class YieldIteratorInfluencedReadOnlyListExtensions
    {
        public static IList<TItem> AsIList<TItem>(this IList<TItem> list) =>
            list;

        public static YieldIteratorInfluencedReadOnlyList<ItemType> ToYieldIteratorInfluencedReadOnlyList<ItemType>(this IList<ItemType> list) =>
            new YieldIteratorInfluencedReadOnlyList<ItemType>.List(list);

        public static IReadOnlyList<ItemType> AsIReadOnlyList<ItemType>(this IReadOnlyList<ItemType> list) =>
            list;

        public static YieldIteratorInfluencedReadOnlyList<ItemType> ToYieldIteratorInfluencedReadOnlyList<ItemType>(this IReadOnlyList<ItemType> list) =>
            new YieldIteratorInfluencedReadOnlyList<ItemType>.ReadOnlyList(list);
    }
}
