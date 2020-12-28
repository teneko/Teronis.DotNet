using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Changes;
using Teronis.ObjectModel.Parenting;

namespace Teronis.Collections.Synchronization
{
    public class AddRemoveResetBehaviourForCollectionItemByAddRemoveParents<ItemType, ContentType>
        where ItemType : IHaveParents
    {
        public INotifyCollectionModified<ItemType, ContentType> CollectionModifiedNotifier { get; private set; }
        public ReadOnlyCollection<object> Parents { get; private set; }

        public AddRemoveResetBehaviourForCollectionItemByAddRemoveParents(INotifyCollectionModified<ItemType, ContentType> collectionModifiedNotifier, params object[] parents)
        {
            CollectionModifiedNotifier = collectionModifiedNotifier;
            CollectionModifiedNotifier.CollectionModified += CollectionModifiedNotifier_CollectionModified;
            var parentList = new List<object>(parents);
            Parents = new ReadOnlyCollection<object>(parentList);
        }

        public AddRemoveResetBehaviourForCollectionItemByAddRemoveParents(INotifyCollectionModified<ItemType, ContentType> collectionModifiedNotifier, bool collectionModifiedNotifierIsParent)
            : this(collectionModifiedNotifier, collectionModifiedNotifierIsParent ? new object[] { collectionModifiedNotifier } : new object[] { }) { }

        private void Item_WantParent(object sender, HavingParentsEventArgs e)
            => e.AddParentsAndTheirParents(Parents);

        private void addParentWhenRequestedHandler(ItemType item)
            => item.ParentsRequested += Item_WantParent;

        private void removeParentWhenRequestedHandler(ItemType item)
            => item.ParentsRequested -= Item_WantParent;

        private void CollectionModifiedNotifier_CollectionModified(object sender, CollectionModifiedEventArgs<ItemType, ContentType> args)
        {
            var change = args.OldSubItemsNewSubItemsModification;

            var oldItemItemItems = new Lazy<IReadOnlyList<ItemType>>(() => change.OldItems ??
                throw new ArgumentException("The old item-item-items were not given that can be processed as collection change"));

            var newItemItemItems = new Lazy<IReadOnlyList<ItemType>>(() => change.NewItems ??
                throw new ArgumentException("The new item-item-items were not given that can be processed as collection change"));

            switch (change.Action) {
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in oldItemItemItems.Value) {
                        removeParentWhenRequestedHandler(item);
                    }

                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in newItemItemItems.Value) {
                        addParentWhenRequestedHandler(item);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var oldItem in oldItemItemItems.Value) {
                        removeParentWhenRequestedHandler(oldItem);
                    }

                    foreach (var newItem in newItemItemItems.Value) {
                        addParentWhenRequestedHandler(newItem);
                    }

                    break;
            }
        }
    }
}
