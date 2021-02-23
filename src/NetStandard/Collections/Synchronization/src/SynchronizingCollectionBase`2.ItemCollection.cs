using System;
using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizingCollectionBase<SuperItemType, SubItemType>
    {
        public abstract partial class ItemCollection<ItemType, NewItemType> : SynchronizableCollectionBase<ItemType, NewItemType>
        {
            public ItemCollection(IList<ItemType> list, SynchronizingCollectionBase<SuperItemType, SubItemType> synchronizingCollection)
                : base(list)
            {
                synchronizingCollection.CollectionSynchronizing += SynchronizingCollection_CollectionSynchronizing;
                synchronizingCollection.CollectionModified += CollectionModificationNotifier_CollectionModified;
                synchronizingCollection.CollectionSynchronized += SynchronizingCollection_CollectionSynchronized;
            }

            private void SynchronizingCollection_CollectionSynchronizing(object? sender, EventArgs e) =>
                InvokeCollectionSynchronizing();

            protected abstract ICollectionModification<ItemType, ItemType> GetCollectionModification(CollectionModifiedEventArgs<SuperItemType, SubItemType> args);

            private void CollectionModificationNotifier_CollectionModified(object? sender, CollectionModifiedEventArgs<SuperItemType, SubItemType> args)
            {
                var collectionModification = GetCollectionModification(args);
                InvokeCollectionModified(collectionModification);
            }

            private void SynchronizingCollection_CollectionSynchronized(object? sender, EventArgs e) =>
                InvokeCollectionSynchronized();
        }

        public class SubItemCollection : ItemCollection<SubItemType, SuperItemType>
        {
            public SubItemCollection(IList<SubItemType> list, SynchronizingCollectionBase<SuperItemType, SubItemType> synchronizingCollection)
                : base(list, synchronizingCollection) { }

            protected override ICollectionModification<SubItemType, SubItemType> GetCollectionModification(CollectionModifiedEventArgs<SuperItemType, SubItemType> args) =>
                args.SubItemModification;
        }

        public class SuperItemCollection : ItemCollection<SuperItemType, SubItemType>
        {
            public SuperItemCollection(IList<SuperItemType> list, SynchronizingCollectionBase<SuperItemType, SubItemType> synchronizingCollection)
                : base(list, synchronizingCollection) { }

            protected override ICollectionModification<SuperItemType, SuperItemType> GetCollectionModification(CollectionModifiedEventArgs<SuperItemType, SubItemType> args) =>
                args.SuperItemModification;
        }
    }
}
