using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SyncingCollectionViewModel<SubItemType, SuperItemType>
    {
        public abstract partial class ItemCollection<ItemType> : Collection<ItemType>, ISynchronizableItemCollection<ItemType>
        {
            public event EventHandler? CollectionSynchronizing;

            public event NotifyNotifyCollectionModifiedEventHandler<ItemType>? CollectionModified;

            private bool itemTypeImplementsDisposable;

            event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged {
                add => collectionChanged += value;
                remove => collectionChanged -= value;
            }

            public event EventHandler? CollectionSynchronized;

            private event NotifyCollectionChangedEventHandler? collectionChanged;

            new internal IList<ItemType> Items => base.Items;

            public ItemCollection(SyncingCollectionViewModel<SubItemType, SuperItemType> synchronizingCollection)
            {
                itemTypeImplementsDisposable = typeof(IDisposable).IsAssignableFrom(typeof(ItemType));
                synchronizingCollection.CollectionSynchronizing += SyncingCollectionViewModel_CollectionSynchronizing;
                synchronizingCollection.CollectionModified += CollectionModificationNotifier_CollectionModified;
                synchronizingCollection.CollectionSynchronized += SyncingCollectionViewModel_CollectionSynchronized;
            }

            public KeyedItemIndexTracker<ItemType, KeyType> CreateKeyedItemIndexTracker<KeyType>(Func<ItemType, KeyType> getItemKey, IEqualityComparer<KeyType> keyEqualityComparer) =>
                new KeyedItemIndexTracker<ItemType, KeyType>(this, getItemKey, keyEqualityComparer);

            public KeyedItemIndexTracker<ItemType, KeyType> CreateKeyedItemIndexTracker<KeyType>(Func<ItemType, KeyType> getItemKey) =>
                new KeyedItemIndexTracker<ItemType, KeyType>(this, getItemKey);

            private void SyncingCollectionViewModel_CollectionSynchronizing(object? sender, EventArgs e) =>
                CollectionSynchronizing?.Invoke(this, e);

            protected abstract CollectionModifiedEventArgs<ItemType> CreateCollectionModifiedEventArgs(CollectionModifiedEventArgs<SubItemType, SuperItemType> args);

            private void CollectionModificationNotifier_CollectionModified(object? sender, CollectionModifiedEventArgs<SubItemType, SuperItemType> args)
            {
                if (collectionChanged is null && CollectionModified is null) {
                    return;
                }

                var collectionChangedEventArgs = CreateCollectionModifiedEventArgs(args);
                CollectionModified?.Invoke(this, collectionChangedEventArgs);
                collectionChanged?.Invoke(this, collectionChangedEventArgs);
            }

            private void SyncingCollectionViewModel_CollectionSynchronized(object? sender, EventArgs e) =>
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

        public class SubItemCollection : ItemCollection<SubItemType>
        {
            public SubItemCollection(SyncingCollectionViewModel<SubItemType, SuperItemType> synchronizingCollection)
                : base(synchronizingCollection) { }

            protected override CollectionModifiedEventArgs<SubItemType> CreateCollectionModifiedEventArgs(CollectionModifiedEventArgs<SubItemType, SuperItemType> args) =>
                new CollectionModifiedEventArgs<SubItemType>(args.SubItemModification);
        }

        public class SuperItemCollection : ItemCollection<SuperItemType>
        {
            public SuperItemCollection(SyncingCollectionViewModel<SubItemType, SuperItemType> synchronizingCollection)
                : base(synchronizingCollection) { }

            protected override CollectionModifiedEventArgs<SuperItemType> CreateCollectionModifiedEventArgs(CollectionModifiedEventArgs<SubItemType, SuperItemType> args) =>
                new CollectionModifiedEventArgs<SuperItemType>(args.SuperItemModification);
        }
    }
}
