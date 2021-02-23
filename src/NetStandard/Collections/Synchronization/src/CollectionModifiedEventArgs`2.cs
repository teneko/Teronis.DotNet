using System;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public class CollectionModifiedEventArgs<SuperItemType, SubItemType> : EventArgs, ICollectionModificationBundle<SuperItemType, SubItemType>
    {
        public ICollectionModification<SubItemType, SubItemType> SubItemModification { get; }
        public ICollectionModification<SuperItemType, SubItemType> SubSuperItemModification { get; }
        public ICollectionModification<SuperItemType, SuperItemType> SuperItemModification { get; }

        public CollectionModifiedEventArgs(
            ICollectionModification<SubItemType, SubItemType> subItemModification,
            ICollectionModification<SuperItemType, SubItemType> subSuperItemModification,
            ICollectionModification<SuperItemType, SuperItemType> superItemModification)
        {
            SubItemModification = subItemModification;
            SubSuperItemModification = subSuperItemModification;
            SuperItemModification = superItemModification;
        }
    }
}
