using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Changes;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizingCollection<SubItemType, SuperItemType> where SubItemType : notnull
        where SuperItemType : notnull
    {
        public abstract class ItemCollection<ItemType> : Collection<ItemType>, INotifyCollectionSynchronizing<ItemType>, INotifyCollectionModified<ItemType>, INotifyCollectionChanged, INotifyCollectionSynchronized<ItemType>
        {
            public event EventHandler? CollectionSynchronizing;

            public event NotifyNotifyCollectionModifiedEventHandler<ItemType>? CollectionModified;

            private bool itemTypeImplementsDisposable;

            event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged {
                add => collectionChanged += value;
                remove => collectionChanged -= value;
            }

            public event EventHandler? CollectionSynchronized;

            private event NotifyCollectionChangedEventHandler? collectionChanged;

            public ItemCollection(SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection)
            {
                itemTypeImplementsDisposable = typeof(IDisposable).IsAssignableFrom(typeof(ItemType));
                synchronizingCollection.CollectionSynchronizing += SynchronizingCollection_CollectionSynchronizing;
                synchronizingCollection.CollectionModified += CollectionModificationNotifier_CollectionModified;
                synchronizingCollection.CollectionSynchronized += SynchronizingCollection_CollectionSynchronized;
            }

            private void SynchronizingCollection_CollectionSynchronizing(object sender, EventArgs e) =>
                CollectionSynchronizing?.Invoke(this, e);

            protected abstract NotifyCollectionModifiedEventArgs<ItemType> CreateNotifyCollectionModifiedEventArgs(CollectionModifiedEventArgs<SubItemType, SuperItemType> args);

            private void CollectionModificationNotifier_CollectionModified(object sender, CollectionModifiedEventArgs<SubItemType, SuperItemType> args)
            {
                if (collectionChanged is null && CollectionModified is null) {
                    return;
                }

                var collectionChangedEventArgs = CreateNotifyCollectionModifiedEventArgs(args);
                CollectionModified?.Invoke(this, collectionChangedEventArgs);
                collectionChanged?.Invoke(this, collectionChangedEventArgs);
            }

            private void SynchronizingCollection_CollectionSynchronized(object sender, EventArgs e) =>
                CollectionSynchronized?.Invoke(this, e);

            protected virtual void DisposeItem(int index)
            {
                var item = Items[index];

                if (item is IDisposable disposableItem) {
                    disposableItem.Dispose();
                }
            }

            protected override void RemoveItem(int index)
            {
                if (itemTypeImplementsDisposable) {
                    DisposeItem(index);
                }

                base.RemoveItem(index);
            }
        }
    }
}
