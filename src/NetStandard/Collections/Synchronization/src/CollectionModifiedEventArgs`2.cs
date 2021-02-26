using System;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public class CollectionModifiedEventArgs<SuperItemType, SubItemType> : EventArgs
    {
        public ICollectionModification<SubItemType, SubItemType> SubItemModification { get; }
        public ICollectionModification<SuperItemType, SubItemType> SuperSubItemModification { get; }
        public ICollectionModification<SuperItemType, SuperItemType> SuperItemModification { get; }

        public CollectionModifiedEventArgs(
            ICollectionModification<SubItemType, SubItemType> subItemModification,
            ICollectionModification<SuperItemType, SubItemType> superSubItemModification,
            ICollectionModification<SuperItemType, SuperItemType> superItemModification)
        {
            SubItemModification = subItemModification;
            SuperSubItemModification = superSubItemModification;
            SuperItemModification = superItemModification;
        }
    }
}
