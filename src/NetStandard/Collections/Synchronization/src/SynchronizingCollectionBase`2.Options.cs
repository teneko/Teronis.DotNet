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

                public void SetItems(
                    ISynchronizedCollection<ItemType> items,
                    CollectionChangeHandler<ItemType>.IDependencyInjectedHandler modificationHandler)
                {
                    Items = items ?? throw new ArgumentNullException(nameof(items));
                    CollectionChangeHandler = modificationHandler ?? throw new ArgumentNullException(nameof(modificationHandler));
                }

                public void SetItems(CollectionChangeHandler<ItemType>.DecoupledHandler modificationHandler)
                {
                    if (modificationHandler is null) {
                        throw new ArgumentNullException(nameof(modificationHandler));
                    }

                    var list = new List<ItemType>();
                    CollectionChangeHandler = new CollectionChangeHandler<ItemType>.DependencyInjectedHandler(list, modificationHandler);
                }
            }

            public sealed class OptionsForSuperItems : ItemsOptions<SuperItemType> {
                /// <summary>
                /// If not null it is called in <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItemByModification(ApplyingCollectionModifications)"/>
                /// but after the items could have been replaced.
                /// </summary>
                public CollectionUpdateItemDelegate<SuperItemType, SubItemType>? UpdateItem { get; set; }
            }

            public sealed class OptionsForSubItems : ItemsOptions<SubItemType> {
                /// <summary>
                /// If not null it is called by <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItemByModification(ApplyingCollectionModifications)"/>
                /// but after the items could have been replaced.
                /// <br/>
                /// <br/>(!) Take into regard, that <see cref="UpdateSuperItem"/> is called at first if not null.
                /// </summary>
                public CollectionUpdateItemDelegate<SubItemType, SuperItemType>? UpdateItem { get; set; }
            }
        }
    }
}
