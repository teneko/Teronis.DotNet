using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public partial class SynchronizingCollectionBase<SuperItemType, SubItemType>
    {
        public sealed class Options
        {
            public OptionsForSubItems SubItemsOptions { get; }
            public OptionsForSuperItems SuperItemsOptions { get; }
            public ICollectionSynchronizationMethod<SuperItemType, SuperItemType>? SynchronizationMethod { get; set; }

            public Options()
            {
                SuperItemsOptions = new OptionsForSuperItems();
                SubItemsOptions = new OptionsForSubItems();
            }

            public Options SetSequentialSynchronizationMethod(IEqualityComparer<SuperItemType> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sequential(equalityComparer);
                return this;
            }

            public Options SetAscendedSynchronizationMethod(IComparer<SuperItemType> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Ascending(equalityComparer);
                return this;
            }

            public Options SetDescendedSynchronizationMethod(IComparer<SuperItemType> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Descending(equalityComparer);
                return this;
            }

            public Options SetSortedSynchronizationMethod(IComparer<SuperItemType> equalityComparer, bool descended)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sorted(equalityComparer, descended);
                return this;
            }

            public abstract class ItemsOptions<ItemType>
            {
                public ISynchronizedCollection<ItemType>? Items { get; private set; }
                public CollectionChangeHandler<ItemType>.IDependencyInjectedHandler? CollectionChangeHandler { get; private set; }

                /// <summary>
                /// Sets <see cref="Items"/> and <see cref="CollectionChangeHandler"/>.
                /// </summary>
                /// <param name="items"></param>
                /// <param name="modificationHandler"></param>
                public void SetItems(
                    ISynchronizedCollection<ItemType> items,
                    CollectionChangeHandler<ItemType>.IDependencyInjectedHandler modificationHandler)
                {
                    Items = items ?? throw new ArgumentNullException(nameof(items));
                    CollectionChangeHandler = modificationHandler ?? throw new ArgumentNullException(nameof(modificationHandler));
                }

                /// <summary>
                /// Sets <see cref="CollectionChangeHandler"/> by creating a 
                /// <see cref="CollectionChangeHandler{ItemType}.DependencyInjectedHandler"/>
                /// with <paramref name="modificationHandlerItems"/> and <paramref name="modificationHandler"/>.
                /// The <see cref="Items"/> remains null and is initialized at contruction of
                /// <see cref="SynchronizableCollectionBase{ItemType, NewItemType}"/>.
                /// </summary>
                /// <param name="modificationHandler"></param>
                public void SetItems(IList<ItemType> modificationHandlerItems, CollectionChangeHandler<ItemType>.IDecoupledHandler modificationHandler)
                {
                    if (modificationHandlerItems is null) {
                        throw new ArgumentNullException(nameof(modificationHandlerItems));
                    }

                    if (modificationHandler is null) {
                        throw new ArgumentNullException(nameof(modificationHandler));
                    }

                    CollectionChangeHandler = new CollectionChangeHandler<ItemType>.DependencyInjectedHandler(modificationHandlerItems, modificationHandler);
                }

                /// <summary>
                /// Sets <see cref="CollectionChangeHandler"/> by creating a 
                /// <see cref="CollectionChangeHandler{ItemType}.DependencyInjectedHandler"/>
                /// with new <see cref="List{T}"/> and <paramref name="modificationHandler"/>.
                /// The <see cref="Items"/> remains null and is initialized at contruction of
                /// <see cref="SynchronizableCollectionBase{ItemType, NewItemType}"/>.
                /// </summary>
                /// <param name="modificationHandler"></param>
                public void SetItems(CollectionChangeHandler<ItemType>.IDecoupledHandler modificationHandler) =>
                    SetItems(new List<ItemType>(), modificationHandler);
            }

            public sealed class OptionsForSuperItems : ItemsOptions<SuperItemType> {
                /// <summary>
                /// If not null it is called in <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItemByModification(ApplyingCollectionModifications)"/>
                /// but after the items could have been replaced and before <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.OnReplacedItemByModification(int)"/>.
                /// </summary>
                public CollectionUpdateItemDelegate<SuperItemType, SubItemType>? UpdateItem { get; set; }
            }

            public sealed class OptionsForSubItems : ItemsOptions<SubItemType> {
                /// <summary>
                /// If not null it is called by <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItemByModification(ApplyingCollectionModifications)"/>
                /// but after the items could have been replaced and before <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.OnReplacedItemByModification(int)"/>.
                /// <br/>
                /// <br/>(!) Take into regard, that <see cref="OptionsForSuperItems.UpdateItem"/> is called at first if not null.
                /// </summary>
                public CollectionUpdateItemDelegate<SubItemType, SuperItemType>? UpdateItem { get; set; }
            }
        }
    }
}
