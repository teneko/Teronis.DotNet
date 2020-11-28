using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Changes;
using Teronis.Data;
using Teronis.Extensions;

namespace Teronis.Collections.Synchronization
{
    public class CollectionItemParentsBehaviour<ItemType, ContentType>
        where ItemType : IHaveParents
    {
        public INotifyCollectionChangeApplied<ItemType, ContentType> CollectionChangeAppliedNotifier { get; private set; }
        public ReadOnlyCollection<object> Parents { get; private set; }

        public CollectionItemParentsBehaviour(INotifyCollectionChangeApplied<ItemType, ContentType> collectionChangeAppliedNotifier, params object[] parents)
        {
            CollectionChangeAppliedNotifier = collectionChangeAppliedNotifier;
            CollectionChangeAppliedNotifier.CollectionChangeApplied += NotifiableCollectionContainer_CollectionChanged;
            var parentCollection = new List<object>(parents) { collectionChangeAppliedNotifier };
            Parents = new ReadOnlyCollection<object>(parentCollection);
        }

        private void Item_WantParent(object sender, HavingParentsEventArgs e)
            => e.AttachParentsParents(Parents);

        private void attachWantParentsHandler(ItemType item)
            => item.WantParents += Item_WantParent;

        private void detachWantParentsHandler(ItemType item)
            => item.WantParents -= Item_WantParent;

        private void NotifiableCollectionContainer_CollectionChanged(object sender, CollectionChangeAppliedEventArgs<ItemType, ContentType> args)
        {
            var change = args.ItemItemChange;

            var oldItemItemItems = new Lazy<IReadOnlyList<ItemType>>(() => change.OldItems ??
                throw new ArgumentException("The old item-item-items were not given that can be processed as collection change"));

            var newItemItemItems = new Lazy<IReadOnlyList<ItemType>>(() => change.NewItems ??
                throw new ArgumentException("The new item-item-items were not given that can be processed as collection change"));

            switch (change.Action) {
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in oldItemItemItems.Value) {
                        detachWantParentsHandler(item);
                    }

                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in newItemItemItems.Value) {
                        attachWantParentsHandler(item);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var oldItem in oldItemItemItems.Value) {
                        detachWantParentsHandler(oldItem);
                    }

                    foreach (var newItem in newItemItemItems.Value) {
                        attachWantParentsHandler(newItem);
                    }

                    break;
            }
        }
    }
}
