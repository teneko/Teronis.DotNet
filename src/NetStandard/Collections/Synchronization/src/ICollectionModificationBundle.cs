using Teronis.Collections.Algorithms;

namespace Teronis.Collections.Synchronization
{
    internal interface ICollectionModificationBundle<out SubItemType, out SuperItemType>
    {
        ICollectionModification<SubItemType, SubItemType> OldSubItemsNewSubItemsModification { get; }
        ICollectionModification<SubItemType, SuperItemType> OldSubItemsNewSuperItemsModification { get; }
        ICollectionModification<SuperItemType, SuperItemType> OldSuperItemsNewSuperItemsModification { get; }
    }
}
