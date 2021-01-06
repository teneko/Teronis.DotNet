using Teronis.Collections.Changes;

namespace Teronis.Collections.Synchronization
{
    public abstract class CollectionItemReplaceStrategyBase
    {
        public abstract void ApplyCollectionItemReplace<SubItemType, SuperItemType>(
            SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection,
            ICollectionModification<SubItemType, SuperItemType> applyingOldSubNewSuperModification,
            ICollectionModification<SuperItemType, SuperItemType> applyingOldSuperNewSuperModification)
            where SubItemType : notnull
            where SuperItemType : notnull;
    }
}
