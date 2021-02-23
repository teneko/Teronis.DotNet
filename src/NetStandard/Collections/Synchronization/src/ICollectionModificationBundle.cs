using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    internal interface ICollectionModificationBundle<out SuperItemType, out SubItemType>
    {
        ICollectionModification<SubItemType, SubItemType> SubItemModification { get; }
        ICollectionModification<SuperItemType, SubItemType> SubSuperItemModification { get; }
        ICollectionModification<SuperItemType, SuperItemType> SuperItemModification { get; }
    }
}
