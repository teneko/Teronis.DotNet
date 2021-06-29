// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class CollectionModification
    {
        public static CollectionModification<TNewItem, TOldItem> ForAdd<TNewItem, TOldItem>(int newIndex, TNewItem newItem) =>
            new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Add, oldItem: default!, oldIndex: -1, newItem, newIndex);

        public static CollectionModification<TNewItem, TOldItem> ForAdd<TNewItem, TOldItem>(int newIndex, IReadOnlyList<TNewItem> newItems) =>
            new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Add, oldItems: null, oldIndex: -1, newItems, newIndex);

        public static CollectionModification<TNewItem, TOldItem> ForRemove<TNewItem, TOldItem>(int oldIndex, TOldItem oldItem) =>
               new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Remove, oldItem, oldIndex, newItems: default, newIndex: -1);

        public static CollectionModification<TNewItem, TOldItem> ForRemove<TNewItem, TOldItem>(int oldIndex, IReadOnlyList<TOldItem> oldItems) =>
            new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Remove, oldItems, oldIndex, newItem: default, newIndex: -1);

        #region Replace (indexed)

        public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(int oldIndex, TOldItem oldItem, TNewItem newItem) =>
               new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItem, oldIndex, newItem, oldIndex);

        public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(int oldIndex, IReadOnlyList<TOldItem> oldItems, IReadOnlyList<TNewItem> newItems) =>
            new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItems, oldIndex, newItems, oldIndex);

        public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(int oldIndex, TOldItem oldItem, IReadOnlyList<TNewItem> newItems) =>
               new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItem, oldIndex, newItems, oldIndex);

        public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(int oldIndex, IReadOnlyList<TOldItem> oldItems, TNewItem newItem) =>
            new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItems, oldIndex, newItem, oldIndex);

        #endregion

        #region Replace (non-indexed)

        /* TODO: Allow replacable non-indexed old items with new items. */

        //public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(TOldItem oldItem, TNewItem newItem) =>
        //       new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItem, oldIndex: -1, newItem, newIndex: -1);

        //public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(IReadOnlyList<TOldItem> oldItems, IReadOnlyList<TNewItem> newItems) =>
        //    new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItems, oldIndex: -1, newItems, newIndex: -1);

        //public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(TOldItem oldItem, IReadOnlyList<TNewItem> newItems) =>
        //       new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItem, oldIndex: -1, newItems, newIndex: -1);

        //public static CollectionModification<TNewItem, TOldItem> ForReplace<TNewItem, TOldItem>(IReadOnlyList<TOldItem> oldItems, TNewItem newItem) =>
        //    new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Replace, oldItems, oldIndex: -1, newItem, newIndex: -1);

        #endregion

        public static CollectionModification<TNewItem, TOldItem> ForMove<TNewItem, TOldItem>(int oldIndex, TOldItem oldItem, int newIndex) =>
               new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Move, oldItem, oldIndex, newItem: default, newIndex);

        private static IReadOnlyList<TItem> CutOut<TItem>(int index, IEnumerable<TItem> originalItems, int cutOutSize)
        {
            if (originalItems is List<TItem> list) {
                var items = new TItem[cutOutSize];
                list.CopyTo(index, items, 0, cutOutSize);
                return items;
            } else {
                return originalItems.Skip(index).Take(cutOutSize).ToArray();
            }
        }

        public static CollectionModification<TNewItem, TOldItem> ForMove<TNewItem, TOldItem>(int oldIndex, IReadOnlyList<TOldItem> oldItems, int newIndex) =>
            new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Move, oldItems, oldIndex, newItems: null, newIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNewItem"></typeparam>
        /// <typeparam name="TOldItem"></typeparam>
        /// <param name="oldIndex"></param>
        /// <param name="oldItems"></param>
        /// <param name="newIndex"></param>
        /// <param name="oldItemsCutOutSize">
        /// If specified it means, that <paramref name="oldItems"/> is the 
        /// whole and  original item list of which the items are cutted out
        /// by the amount of <paramref name="oldItemsCutOutSize"/>.
        /// </param>
        /// <returns></returns>
        public static CollectionModification<TNewItem, TOldItem> ForMove<TNewItem, TOldItem>(int oldIndex, IEnumerable<TOldItem> oldItems, int newIndex, int oldItemsCutOutSize)
        {
            var oldItemList = CutOut(oldIndex, oldItems, oldItemsCutOutSize);
            return new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Move, oldItemList, oldIndex, newItems: null, newIndex);
        }

        public static CollectionModification<TNewItem, TOldItem> ForReset<TNewItem, TOldItem>() =>
            new CollectionModification<TNewItem, TOldItem>(NotifyCollectionChangedAction.Reset, oldItems: null, oldIndex: -1, newItems: null, newIndex: -1);
    }
}
