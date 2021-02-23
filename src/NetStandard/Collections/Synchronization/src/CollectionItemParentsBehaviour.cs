using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Algorithms;
using Teronis.ObjectModel.Parenthood;

namespace Teronis.Collections.Synchronization
{
    public class AddRemoveResetBehaviourForCollectionItemByAddRemoveParents<ContentType, ItemType>
        where ItemType : IHaveParents
    {
        public INotifyCollectionModification<ContentType, ItemType> CollectionModifiedNotifier { get; private set; }
        public ReadOnlyCollection<object> Parents { get; private set; }

        public AddRemoveResetBehaviourForCollectionItemByAddRemoveParents(INotifyCollectionModification<ContentType, ItemType> collectionModifiedNotifier, params object[] parents)
        {
            CollectionModifiedNotifier = collectionModifiedNotifier;
            CollectionModifiedNotifier.CollectionModified += CollectionModifiedNotifier_CollectionModified;
            var parentList = new List<object>(parents);
            Parents = new ReadOnlyCollection<object>(parentList);
        }

        public AddRemoveResetBehaviourForCollectionItemByAddRemoveParents(INotifyCollectionModification<ContentType, ItemType> collectionModifiedNotifier, bool collectionModifiedNotifierIsParent)
            : this(collectionModifiedNotifier, collectionModifiedNotifierIsParent ? new object[] { collectionModifiedNotifier } : new object[] { }) { }

        private void Item_WantParent(object sender, HavingParentsEventArgs e)
            => e.AddParentsAndTheirParents(Parents);

        private void addParentWhenRequestedHandler(ItemType item)
            => item.ParentsRequested += Item_WantParent;

        private void removeParentWhenRequestedHandler(ItemType item)
            => item.ParentsRequested -= Item_WantParent;

        private void CollectionModifiedNotifier_CollectionModified(object sender, CollectionModifiedEventArgs<ContentType, ItemType> args)
        {
            var change = args.SubItemModification;

            var oldItemItemItems = new Lazy<IReadOnlyList<ItemType>>(() => change.OldItems ??
                throw CollectionModificationThrowHelper.OldItemsWereNullException());

            var newItemItemItems = new Lazy<IReadOnlyList<ItemType>>(() => change.NewItems ??
                throw CollectionModificationThrowHelper.NewItemsWereNullException());

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
