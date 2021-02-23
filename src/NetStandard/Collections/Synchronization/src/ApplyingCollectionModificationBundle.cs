using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    internal readonly struct ApplyingCollectionModificationBundle<SuperItemType, SubItemType>
    {
        public ICollectionModification<SuperItemType, SubItemType> OldSubItemsNewSuperItemsModification { get; }
        public ICollectionModification<SuperItemType, SuperItemType> OldSuperItemsNewSuperItemsModification { get; }

        public ApplyingCollectionModificationBundle(ICollectionModification<SuperItemType, SubItemType> oldSubItemsNewSuperItemsModification,
            ICollectionModification<SuperItemType, SuperItemType> oldSuperItemsNewSuperItemsModification)
        {
            OldSubItemsNewSuperItemsModification = oldSubItemsNewSuperItemsModification;
            OldSuperItemsNewSuperItemsModification = oldSuperItemsNewSuperItemsModification;
        }
    }
}
