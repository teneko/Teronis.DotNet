// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    internal static class ProducedListModificationsNotBatchedMarkerExtensions
    {
        public static IList<TItem> AsIList<TItem>(this IList<TItem> list) =>
            list;

        /// <summary>
        /// Wraps <paramref name="list"/> to mark it. Indicates that produced collection modifications are not batched.
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ProducedListModificationsNotBatchedMarker<ItemType> ToProducedListModificationsNotBatchedMarker<ItemType>(this IList<ItemType> list) =>
            new ProducedListModificationsNotBatchedMarker<ItemType>.List(list);

        public static IReadOnlyList<ItemType> AsIReadOnlyList<ItemType>(this IReadOnlyList<ItemType> list) =>
            list;

        /// <summary>
        /// Wraps <paramref name="list"/> to mark it. Indicates that produced collection modifications are not batched.
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ProducedListModificationsNotBatchedMarker<ItemType> ToProducedListModificationsNotBatchedMarker<ItemType>(this IReadOnlyList<ItemType> list) =>
            new ProducedListModificationsNotBatchedMarker<ItemType>.ReadOnlyList(list);
    }
}
