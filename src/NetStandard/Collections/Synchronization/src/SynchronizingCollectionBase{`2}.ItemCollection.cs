// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizingCollectionBase<TSuperItem, TSubItem>
    {
        public abstract partial class ItemCollection<TItem, TNewItem> : SynchronizableCollectionBase<TItem, TNewItem>
        {
            public ItemCollection(IList<TItem> items, ISynchronizableCollectionItemsOptions<TItem> options, SynchronizingCollectionBase<TSuperItem, TSubItem> synchronizingCollection)
                : base(items, options)
            {
                synchronizingCollection.CollectionSynchronizing += SynchronizingCollection_CollectionSynchronizing;
                synchronizingCollection.CollectionModified += CollectionModificationNotifier_CollectionModified;
                synchronizingCollection.CollectionSynchronized += SynchronizingCollection_CollectionSynchronized;
            }

            private void SynchronizingCollection_CollectionSynchronizing(object? sender, EventArgs e) =>
                OnCollectionSynchronizing();

            protected abstract ICollectionModification<TItem, TItem> GetCollectionModification(CollectionModifiedEventArgs<TSuperItem, TSubItem> args);

            private void CollectionModificationNotifier_CollectionModified(object? sender, CollectionModifiedEventArgs<TSuperItem, TSubItem> args)
            {
                var collectionModification = GetCollectionModification(args);
                OnCollectionModified(collectionModification);
            }

            private void SynchronizingCollection_CollectionSynchronized(object? sender, EventArgs e) =>
                OnCollectionSynchronized();
        }

        public class SubItemCollection : ItemCollection<TSubItem, TSuperItem>
        {
            public SubItemCollection(IList<TSubItem> items, ISynchronizableCollectionItemsOptions<TSubItem> options, SynchronizingCollectionBase<TSuperItem, TSubItem> synchronizingCollection)
                : base(items, options, synchronizingCollection) { }

            protected override ICollectionModification<TSubItem, TSubItem> GetCollectionModification(CollectionModifiedEventArgs<TSuperItem, TSubItem> args) =>
                args.SubItemModification;
        }

        public class SuperItemCollection : ItemCollection<TSuperItem, TSubItem>
        {
            public SuperItemCollection(IList<TSuperItem> items, ISynchronizableCollectionItemsOptions<TSuperItem> options, SynchronizingCollectionBase<TSuperItem, TSubItem> synchronizingCollection)
                : base(items, options, synchronizingCollection) { }

            protected override ICollectionModification<TSuperItem, TSuperItem> GetCollectionModification(CollectionModifiedEventArgs<TSuperItem, TSubItem> args) =>
                args.SuperItemModification;
        }
    }
}
