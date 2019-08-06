using System.Collections.Specialized;
using System.Threading.Tasks;
using Teronis.Data;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionItemUpdateBehaviour<TItem>
        where TItem : IUpdatable<TItem>
    {
        public INotifyCollectionChangeApplied<TItem> NotifiableCollectionContainer { get; private set; }

        public CollectionItemUpdateBehaviour(INotifyCollectionChangeApplied<TItem> collectionChangeNotifier)
        {
            NotifiableCollectionContainer = collectionChangeNotifier;
            NotifiableCollectionContainer.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChangeApplied;
        }

        private async Task updateBy(CollectionChange<TItem> change)
        {
            if (change.Action == NotifyCollectionChangedAction.Replace) {
                var oldItemsEnumerator = change.OldItems.GetEnumerator();
                var newItemsEnumerator = change.NewItems.GetEnumerator();

                while (oldItemsEnumerator.MoveNext() && newItemsEnumerator.MoveNext()) {
                    var oldItem = oldItemsEnumerator.Current;
                    var newItem = newItemsEnumerator.Current;
                    var oldItemUpdate = new Update<TItem>(newItem, this);
                    await oldItem.UpdateByAsync(oldItemUpdate);
                }
            }
        }

        private async void NotifiableCollectionContainer_CollectionChangeApplied(object sender, AspectedCollectionChange<TItem> args)
        {
            var change = args.Change;
            await 
        }
    }
}
