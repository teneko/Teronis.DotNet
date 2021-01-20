using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public static class SyncingCollectionViewModelAlignment
    {
        public static SyncingCollectionViewModelAlignment<ItemType> OrderedAlignment<ItemType>(IEqualityComparer<ItemType> equalityComparer) =>
            new SyncingCollectionViewModelAlignment<ItemType>(equalityComparer);

        public static SyncingCollectionViewModelAlignment<ItemType> AscendingAlignment<ItemType>(IComparer<ItemType> comparer) =>
            new SyncingCollectionViewModelAlignment<ItemType>(comparer, SyncingCollectionViewModelOrder.Ascending);

        public static SyncingCollectionViewModelAlignment<ItemType> DescendingAlignment<ItemType>(IComparer<ItemType> comparer) =>
            new SyncingCollectionViewModelAlignment<ItemType>(comparer, SyncingCollectionViewModelOrder.Descending);
    }
}
