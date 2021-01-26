using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    internal interface ICollectionModificationBundle<out SubItemType, out SuperItemType>
    {
        ICollectionModification<SubItemType, SubItemType> SubItemModification { get; }
        ICollectionModification<SubItemType, SuperItemType> SubSuperItemModification { get; }
        ICollectionModification<SuperItemType, SuperItemType> SuperItemModification { get; }
    }
}
