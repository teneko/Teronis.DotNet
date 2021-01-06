using System;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizingCollection<SubItemType, SuperItemType> where SubItemType : notnull
        where SuperItemType : notnull
    {
        //public class ReadOnlyItemCollection<ItemType> : ReadOnlyCollection<ItemType>, INotifyCollectionSynchronizing<ItemType>, INotifyCollectionModified<ItemType>, INotifyCollectionChanged, INotifyCollectionSynchronized<ItemType>
        //{
        //    private readonly SynchronizingCollection<SubItemType, SuperItemType>.ItemCollection<ItemType> list;

        //    public ReadOnlyItemCollection(ItemCollection<ItemType> list)
        //        : base(list) =>
        //        this.list = list;

        //    public event EventHandler CollectionSynchronizing {
        //        add => ((INotifyCollectionSynchronizing<ItemType>)list).CollectionSynchronizing += value;
        //        remove => ((INotifyCollectionSynchronizing<ItemType>)list).CollectionSynchronizing -= value;
        //    }

        //    public event NotifyNotifyCollectionModifiedEventHandler<ItemType> CollectionModified {
        //        add => ((INotifyCollectionModified<ItemType>)list).CollectionModified += value;
        //        remove => ((INotifyCollectionModified<ItemType>)list).CollectionModified -= value;
        //    }

        //    public event NotifyCollectionChangedEventHandler CollectionChanged {
        //        add => ((INotifyCollectionChanged)list).CollectionChanged += value;
        //        remove => ((INotifyCollectionChanged)list).CollectionChanged -= value;
        //    }

        //    public event EventHandler CollectionSynchronized {
        //        add => ((INotifyCollectionSynchronized<ItemType>)list).CollectionSynchronized += value;
        //        remove => ((INotifyCollectionSynchronized<ItemType>)list).CollectionSynchronized -= value;
        //    }
        //}

        public class CollectionModifiedImitator<ToBeImitatedCollectionType>
            where ToBeImitatedCollectionType : INotifyCollectionSynchronizing<SuperItemType>, INotifyCollectionModified<SuperItemType>, INotifyCollectionSynchronized<SuperItemType>
        {
            private readonly SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection;

            public CollectionModifiedImitator(SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection, ToBeImitatedCollectionType toBeImitatedCollection)
            {
                toBeImitatedCollection.CollectionSynchronizing += ToBeImitatedCollection_CollectionSynchronizing;
                toBeImitatedCollection.CollectionModified += ToBeImitatedCollection_CollectionModified;
                toBeImitatedCollection.CollectionSynchronized += ToBeImitatedCollection_CollectionSynchronized;
                this.synchronizingCollection = synchronizingCollection;
            }

            private void ToBeImitatedCollection_CollectionModified(object sender, NotifyCollectionModifiedEventArgs<SuperItemType> e) =>
                synchronizingCollection.ApplyCollectionModification(e);

            private void ToBeImitatedCollection_CollectionSynchronizing(object sender, EventArgs e) =>
                synchronizingCollection.OnCollectionSynchronizing();

            private void ToBeImitatedCollection_CollectionSynchronized(object sender, EventArgs e) =>
                synchronizingCollection.OnCollectionSynchronized();
        }
    }
}
