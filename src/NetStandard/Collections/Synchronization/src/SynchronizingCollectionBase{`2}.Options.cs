// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public partial class SynchronizingCollectionBase<TSuperItem, TSubItem>
    {
        public sealed class Options
        {
            public OptionsForSubItems SubItemsOptions { get; }
            public OptionsForSuperItems SuperItemsOptions { get; }
            public ICollectionSynchronizationMethod<TSuperItem, TSuperItem>? SynchronizationMethod { get; set; }

            public Options()
            {
                SuperItemsOptions = new OptionsForSuperItems();
                SubItemsOptions = new OptionsForSubItems();
            }

            public Options SetSequentialSynchronizationMethod(IEqualityComparer<TSuperItem> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sequential(equalityComparer);
                return this;
            }

            public Options SetAscendedSynchronizationMethod(IComparer<TSuperItem> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Ascending(equalityComparer);
                return this;
            }

            public Options SetDescendedSynchronizationMethod(IComparer<TSuperItem> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Descending(equalityComparer);
                return this;
            }

            public Options SetSortedSynchronizationMethod(IComparer<TSuperItem> equalityComparer, bool descended)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sorted(equalityComparer, descended);
                return this;
            }

            public abstract class ItemsOptions<ItemType>
            {
                public ISynchronizedCollection<ItemType>? Items { get; private set; }
                public ICollectionChangeHandler<ItemType>? CollectionChangeHandler { get; private set; }

                /// <summary>
                /// Sets <see cref="Items"/> and <see cref="CollectionChangeHandler"/>.
                /// </summary>
                /// <param name="items"></param>
                /// <param name="modificationHandler"></param>
                public void SetItems(
                    ISynchronizedCollection<ItemType> items,
                    ICollectionChangeHandler<ItemType> modificationHandler)
                {
                    Items = items ?? throw new ArgumentNullException(nameof(items));
                    CollectionChangeHandler = modificationHandler ?? throw new ArgumentNullException(nameof(modificationHandler));
                }

                /// <summary>
                /// Sets <see cref="CollectionChangeHandler"/> by creating a 
                /// <see cref="CollectionChangeHandler{ItemType}"/>
                /// with <paramref name="modificationHandlerItems"/> and <paramref name="modificationHandler"/>.
                /// The <see cref="Items"/> remains null and is initialized at contruction of
                /// <see cref="SynchronizableCollectionBase{ItemType, NewItemType}"/>.
                /// </summary>
                /// <param name="modificationHandlerItems"></param>
                /// <param name="modificationHandler"></param>
                public void SetItems(IList<ItemType> modificationHandlerItems, CollectionChangeHandler<ItemType>.IBehaviour modificationHandler)
                {
                    if (modificationHandlerItems is null) {
                        throw new ArgumentNullException(nameof(modificationHandlerItems));
                    }

                    if (modificationHandler is null) {
                        throw new ArgumentNullException(nameof(modificationHandler));
                    }

                    CollectionChangeHandler = new CollectionChangeHandler<ItemType>(modificationHandlerItems, modificationHandler);
                }

                /// <summary>
                /// Sets <see cref="CollectionChangeHandler"/> by creating a 
                /// <see cref="CollectionChangeHandler{ItemType}"/>
                /// with new <see cref="List{T}"/> and <paramref name="modificationHandler"/>.
                /// The <see cref="Items"/> remains null and is initialized at contruction of
                /// <see cref="SynchronizableCollectionBase{ItemType, NewItemType}"/>.
                /// </summary>
                /// <param name="modificationHandler"></param>
                public void SetItems(CollectionChangeHandler<ItemType>.IBehaviour modificationHandler) =>
                    SetItems(new List<ItemType>(), modificationHandler);
            }

            public sealed class OptionsForSuperItems : ItemsOptions<TSuperItem> {
                /// <summary>
                /// If not null it is called in <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItems(ApplyingCollectionModifications)"/>
                /// but after the items could have been replaced and before <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.OnAfterReplaceItem(int)"/>.
                /// </summary>
                public CollectionUpdateItemDelegate<TSuperItem, TSubItem>? UpdateItem { get; set; }
            }

            public sealed class OptionsForSubItems : ItemsOptions<TSubItem> {
                /// <summary>
                /// If not null it is called by <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItems(ApplyingCollectionModifications)"/>
                /// but after the items could have been replaced and before <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.OnAfterReplaceItem(int)"/>.
                /// <br/>
                /// <br/>(!) Take into regard, that <see cref="OptionsForSuperItems.UpdateItem"/> is called at first if not null.
                /// </summary>
                public CollectionUpdateItemDelegate<TSubItem, TSuperItem>? UpdateItem { get; set; }
            }
        }
    }
}
