using Teronis.Collections.Changes;
using Teronis.Collections.Synchronization.Extensions;

namespace Teronis.Collections.Synchronization.Strategies
{
    public class ReplaceSuperCollectionItemByNewSuperItemStrategy : CollectionItemReplaceStrategyBase
    {
        public override void ApplyCollectionItemReplace<SubItemType, SuperItemType>(
            SynchronizingCollection<SubItemType, SuperItemType> collection,
            ICollectionModification<SubItemType, SuperItemType> applyingOldSubNewSuperModification,
            ICollectionModification<SuperItemType, SuperItemType> applyingOldSuperNewSuperModification)
        {
            foreach ((int OldIndex, SuperItemType NewItem) in applyingOldSuperNewSuperModification.YieldTuplesForOldIndexNewItemReplace()) {
                collection.SuperItems[OldIndex] = NewItem;
            }
        }
    }
}
