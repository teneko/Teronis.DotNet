using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public static class SynchronizingCollectionAlignment
    {
        public static SynchronizingCollectionAlignment<ItemType> OrderedAlignment<ItemType>(IEqualityComparer<ItemType> equalityComparer) =>
            new SynchronizingCollectionAlignment<ItemType>(equalityComparer);

        public static SynchronizingCollectionAlignment<ItemType> AscendingAlignment<ItemType>(IComparer<ItemType> comparer) =>
            new SynchronizingCollectionAlignment<ItemType>(comparer, SynchronizingCollectionOrder.Ascending);

        public static SynchronizingCollectionAlignment<ItemType> DescendingAlignment<ItemType>(IComparer<ItemType> comparer) =>
            new SynchronizingCollectionAlignment<ItemType>(comparer, SynchronizingCollectionOrder.Descending);
    }
}
