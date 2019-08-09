using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Teronis.Data;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionItemUpdateBehaviour<TItem>
        where TItem : IUpdatableContent<TItem>
    {
        public INotifyCollectionChangeApplied<TItem> NotifiableCollectionContainer { get; private set; }

        public CollectionItemUpdateBehaviour(INotifyCollectionChangeApplied<TItem> collectionChangeNotifier)
        {
            NotifiableCollectionContainer = collectionChangeNotifier;
            NotifiableCollectionContainer.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChangeApplied;
            NotifiableCollectionContainer.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChangeAppliedAsync;
        }

        private IEnumerable<UpdateWithTargetContainer<TItem, TItem>> getOldItemUpdateContainerIterator(CollectionChange<TItem> change)
        {
            if (change.Action == NotifyCollectionChangedAction.Replace) {
                var oldItemsEnumerator = change.OldItems.GetEnumerator();
                var newItemsEnumerator = change.NewItems.GetEnumerator();

                while (oldItemsEnumerator.MoveNext() && newItemsEnumerator.MoveNext()) {
                    var oldItem = oldItemsEnumerator.Current;
                    var newItem = newItemsEnumerator.Current;
                    var oldItemUpdate = new Update<TItem>(newItem, this);

                    var oldItemUpdateContainer = new UpdateWithTargetContainer<TItem, TItem>() {
                        Update = oldItemUpdate,
                        Target = oldItem
                    };

                    yield return oldItemUpdateContainer;
                }
            }
        }

        private void updateOldItems(CollectionChange<TItem> change)
        {
            foreach (var oldItemUpdateContainer in getOldItemUpdateContainerIterator(change))
                oldItemUpdateContainer.Target.UpdateContainerBy(oldItemUpdateContainer.Update);
        }

        private async Task updateOldItemsAsync(CollectionChange<TItem> change)
        {
            foreach (var oldItemUpdateContainer in getOldItemUpdateContainerIterator(change))
                await oldItemUpdateContainer.Target.UpdateContainerByAsync(oldItemUpdateContainer.Update);
        }

        private void NotifiableCollectionContainer_CollectionChangeApplied(object sender, CollectionChangeAppliedEventArgs<TItem> args)
        {
            if (args.EventSequence != null)
                return;

            var change = args.AspectedCollectionChange.Change;
            updateOldItems(change);
        }

        private async void NotifiableCollectionContainer_CollectionChangeAppliedAsync(object sender, CollectionChangeAppliedEventArgs<TItem> args)
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
