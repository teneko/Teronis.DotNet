using System;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public class CollectionModifiedEventArgs<SubItemType, SuperItemType> : EventArgs, ICollectionModificationBundle<SubItemType, SuperItemType>
    {
        public ICollectionModification<SubItemType, SubItemType> SubItemModification { get; }
        public ICollectionModification<SubItemType, SuperItemType> SubSuperItemModification { get; }
        public ICollectionModification<SuperItemType, SuperItemType> SuperItemModification { get; }

        public CollectionModifiedEventArgs(
            ICollectionModification<SubItemType, SubItemType> subItemModification,
            ICollectionModification<SubItemType, SuperItemType> subSuperItemModification,
            ICollectionModification<SuperItemType, SuperItemType> superItemModification)
        {
            SubItemModification = subItemModification;
            SubSuperItemModification = subSuperItemModification;
            SuperItemModification = superItemModification;
        }
    }
}
