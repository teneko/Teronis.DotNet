using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public partial class SynchronizingCollection<SubItemType, SuperItemType>
    {
        public sealed class Options
        {
            public ItemsOptions<SubItemType> SubItemsOptions { get; }
            public ItemsOptions<SuperItemType> SuperItemsOptions { get; }
            public ICollectionSynchronizationMethod<SuperItemType, SuperItemType>? SynchronizationMethod { get; set; }

            public Options()
            {
                SubItemsOptions = new ItemsOptions<SubItemType>();
                SuperItemsOptions = new ItemsOptions<SuperItemType>();
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

            public class ItemsOptions<ItemType>
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
        }
    }
}
