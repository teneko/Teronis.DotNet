// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public partial class SynchronizableCollection<TItem>
    {
        public sealed class Options
        {
            public ICollectionSynchronizationMethod<TItem, TItem>? SynchronizationMethod { get; set; }

            /// <summary>
            /// If not null it is called in <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItemsByModification(SynchronizingCollectionBase{SuperItemType, SubItemType}.ApplyingCollectionModifications)"/>
            /// but after the items could have been replaced.
            /// </summary>
            public CollectionUpdateItemDelegate<TItem, TItem>? UpdateItem { get; set; }
            public CollectionChangeHandler<TItem>.IDependencyInjectedHandler? CollectionChangeHandler { get; set; }

            public Options SetSequentialSynchronizationMethod(IEqualityComparer<TItem> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sequential(equalityComparer);
                return this;
            }

            public Options SetAscendedSynchronizationMethod(IComparer<TItem> comparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Ascending(comparer);
                return this;
            }

            public Options SetDescendedSynchronizationMethod(IComparer<TItem> comparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Descending(comparer);
                return this;
            }

            public Options SetSortedSynchronizationMethod(IComparer<TItem> comparer, bool descended)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sorted(comparer, descended);
                return this;
            }

            public Options SetItems(IList<TItem> items)
            {
                CollectionChangeHandler = new CollectionChangeHandler<TItem>.DependencyInjectedHandler(items);
                return this;
            }

            public Options SetItems(IList<TItem> items, CollectionChangeHandler<TItem>.IDecoupledHandler decoupledHandler)
            {
                CollectionChangeHandler = new CollectionChangeHandler<TItem>.DependencyInjectedHandler(items, decoupledHandler);
                return this;
            }

            public Options SetItems(CollectionChangeHandler<TItem>.IDecoupledHandler decoupledHandler)
            {
                var itemList = new List<TItem>();
                CollectionChangeHandler = new CollectionChangeHandler<TItem>.DependencyInjectedHandler(itemList, decoupledHandler);
                return this;
            }
        }
    }
}
