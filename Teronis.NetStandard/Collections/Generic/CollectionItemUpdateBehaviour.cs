using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Teronis.Data;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionItemUpdateBehaviour<ItemType, ContentType>
        where ItemType : IUpdatableContentBy<ContentType>
    {
        public INotifyCollectionChangeApplied<ItemType> NotifiableCollectionContainer { get; private set; }

        public CollectionItemUpdateBehaviour(INotifyCollectionChangeApplied<ItemType> collectionChangeNotifier)
        {
            NotifiableCollectionContainer = collectionChangeNotifier;
            NotifiableCollectionContainer.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChangeApplied;
            NotifiableCollectionContainer.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChangeAppliedAsync;
        }

        private IEnumerable<UpdateWithTargetContainer<ContentType, ItemType>> getOldItemUpdateContainerIterator(CollectionChange<ItemType> change)
        {
            if (change.Action == NotifyCollectionChangedAction.Replace) {
                var oldItemsEnumerator = change.OldItems.GetEnumerator();
                var newItemsEnumerator = change.NewItems.GetEnumerator();

                while (oldItemsEnumerator.MoveNext() && newItemsEnumerator.MoveNext()) {
                    var oldItem = oldItemsEnumerator.Current;
                    var newItem = newItemsEnumerator.Current;
                    var oldItemUpdate = new Update<ContentType>(newItem, this);

                    var oldItemUpdateContainer = new UpdateWithTargetContainer<ContentType, ItemType>() {
                        Update = oldItemUpdate,
                        Target = oldItem
                    };

                    yield return oldItemUpdateContainer;
                }
            }
        }

        private void updateOldItems(CollectionChange<ItemType> change)
        {
            foreach (var oldItemUpdateContainer in getOldItemUpdateContainerIterator(change))
                oldItemUpdateContainer.Target.UpdateContentBy(oldItemUpdateContainer.Update);
        }

        private async Task updateOldItemsAsync(CollectionChange<ItemType> change)
        {
            foreach (var oldItemUpdateContainer in getOldItemUpdateContainerIterator(change))
                await oldItemUpdateContainer.Target.UpdateContentByAsync(oldItemUpdateContainer.Update);
        }

        private void NotifiableCollectionContainer_CollectionChangeApplied(object sender, CollectionChangeAppliedEventArgs<ItemType> args)
        {
            if (args.EventSequence != null)
                return;

            var change = args.AspectedCollectionChange.Change;
            updateOldItems(change);
        }

        private async void NotifiableCollectionContainer_CollectionChangeAppliedAsync(object sender, CollectionChangeAppliedEventArgs<ItemType> args)
        {
            if (args.EventSequence == null)
                return;

            var change = args.AspectedCollectionChange.Change;
            var tcs = args.EventSequence.RegisterDependency();

            try {
                await updateOldItemsAsync(change);
                tcs.SetResult();
            } catch (Exception error) {
                tcs.SetException(error);
            }
        }
    }
}
