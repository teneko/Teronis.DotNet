using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Data;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionItemParentsBehaviour<TItem>
        where TItem : IHaveParents
    {
        public INotifyCollectionChangeApplied<TItem> CollectionChangeAppliedNotifier { get; private set; }
        public ReadOnlyCollection<object> Parents { get; private set; }

        public CollectionItemParentsBehaviour(INotifyCollectionChangeApplied<TItem> collectionChangeAppliedNotifier, params object[] parents)
        {
            CollectionChangeAppliedNotifier = collectionChangeAppliedNotifier;
            CollectionChangeAppliedNotifier.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChanged;
            var parentCollection = new List<object>(parents) { collectionChangeAppliedNotifier };
            Parents = new ReadOnlyCollection<object>(parentCollection);
        }

        private void Item_WantParent(object sender, HavingParentsEventArgs e)
            => e.AttachParentsParents(Parents);

        private void attachWantParentsHandler(TItem item)
            => item.WantParents += Item_WantParent;

        private void detachWantParentsHandler(TItem item)
            => item.WantParents -= Item_WantParent;

        private void NotifiableCollectionContainer_CollectionChanged(object sender, AspectedCollectionChange<TItem> args)
        {
            var change = args.Change;

            switch (change.Action) {
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in change.OldItems)
                        detachWantParentsHandler(item);

                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in change.NewItems)
                        attachWantParentsHandler(item);

                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var oldItem in change.OldItems)
                        detachWantParentsHandler(oldItem);

                    foreach (var newItem in change.NewItems)
                        attachWantParentsHandler(newItem);

                    break;
            }
        }
    }
}
