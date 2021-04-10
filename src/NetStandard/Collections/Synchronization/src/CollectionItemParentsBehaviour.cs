// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Algorithms;
using Teronis.ComponentModel.Parenthood;

namespace Teronis.Collections.Synchronization
{
    public class AddRemoveResetBehaviourForCollectionItemByAddRemoveParents<TContent, TItem>
        where TItem : IHaveParents
    {
        public INotifyCollectionModification<TContent, TItem> CollectionModifiedNotifier { get; private set; }
        public ReadOnlyCollection<object> Parents { get; private set; }

        public AddRemoveResetBehaviourForCollectionItemByAddRemoveParents(INotifyCollectionModification<TContent, TItem> collectionModifiedNotifier, params object[] parents)
        {
            CollectionModifiedNotifier = collectionModifiedNotifier;
            CollectionModifiedNotifier.CollectionModified += CollectionModifiedNotifier_CollectionModified;
            var parentList = new List<object>(parents);
            Parents = new ReadOnlyCollection<object>(parentList);
        }

        public AddRemoveResetBehaviourForCollectionItemByAddRemoveParents(INotifyCollectionModification<TContent, TItem> collectionModifiedNotifier, bool collectionModifiedNotifierIsParent)
            : this(collectionModifiedNotifier, collectionModifiedNotifierIsParent ? new object[] { collectionModifiedNotifier } : new object[] { }) { }

        private void Item_WantParent(object sender, HavingParentsEventArgs e)
            => e.AddParentsAndTheirParents(Parents);

        private void addParentWhenRequestedHandler(TItem item)
            => item.ParentsRequested += Item_WantParent;

        private void removeParentWhenRequestedHandler(TItem item)
            => item.ParentsRequested -= Item_WantParent;

        private void CollectionModifiedNotifier_CollectionModified(object sender, CollectionModifiedEventArgs<TContent, TItem> args)
        {
            var change = args.SubItemModification;

            var oldItemItemItems = new Lazy<IReadOnlyList<TItem>>(() => change.OldItems ??
                throw CollectionModificationThrowHelper.OldItemsWereNullException());

            var newItemItemItems = new Lazy<IReadOnlyList<TItem>>(() => change.NewItems ??
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
