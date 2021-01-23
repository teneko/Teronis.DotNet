using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    internal readonly struct ApplyingCollectionModificationBundle<SubItemType, SuperItemType>
    {
        public ICollectionModification<SubItemType, SuperItemType> OldSubItemsNewSuperItemsModification { get; }
        public ICollectionModification<SuperItemType, SuperItemType> OldSuperItemsNewSuperItemsModification { get; }

        public ApplyingCollectionModificationBundle(ICollectionModification<SubItemType, SuperItemType> oldSubItemsNewSuperItemsModification,
            ICollectionModification<SuperItemType, SuperItemType> oldSuperItemsNewSuperItemsModification)
        {
            OldSubItemsNewSuperItemsModification = oldSubItemsNewSuperItemsModification;
            OldSuperItemsNewSuperItemsModification = oldSuperItemsNewSuperItemsModification;
        }
    }
}
