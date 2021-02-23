using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public partial class SynchronizableCollection<ItemType>
    {
        public sealed class Options
        {
            public ICollectionSynchronizationMethod<ItemType, ItemType>? SynchronizationMethod { get; set; }

            /// <summary>
            /// If not null it is called in <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItemByModification(ApplyingCollectionModificationBundle)"/>
            /// but after the items could have been replaced.
            /// </summary>
            public CollectionUpdateItemDelegate<ItemType, ItemType>? UpdateItem { get; set; }
            public CollectionChangeHandler<ItemType>.IDependencyInjectedHandler? CollectionChangeHandler { get; set; }

            public Options SetSequentialSynchronizationMethod(IEqualityComparer<ItemType> equalityComparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sequential(equalityComparer);
                return this;
            }

            public Options SetAscendedSynchronizationMethod(IComparer<ItemType> comparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Ascending(comparer);
                return this;
            }

            public Options SetDescendedSynchronizationMethod(IComparer<ItemType> comparer)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Descending(comparer);
                return this;
            }

            public Options SetSortedSynchronizationMethod(IComparer<ItemType> comparer, bool descended)
            {
                SynchronizationMethod = CollectionSynchronizationMethod.Sorted(comparer, descended);
                return this;
            }

            public Options SetItems(IList<ItemType> items)
            {
                CollectionChangeHandler = new CollectionChangeHandler<ItemType>.DependencyInjectedHandler(items);
                return this;
            }

            public Options SetItems(IList<ItemType> items, CollectionChangeHandler<ItemType>.IDecoupledHandler decoupledHandler)
            {
                CollectionChangeHandler = new CollectionChangeHandler<ItemType>.DependencyInjectedHandler(items, decoupledHandler);
                return this;
            }

            public Options SetItems(CollectionChangeHandler<ItemType>.IDecoupledHandler decoupledHandler)
            {
                var itemList = new List<ItemType>();
                CollectionChangeHandler = new CollectionChangeHandler<ItemType>.DependencyInjectedHandler(itemList, decoupledHandler);
                return this;
            }
        }
    }
}
