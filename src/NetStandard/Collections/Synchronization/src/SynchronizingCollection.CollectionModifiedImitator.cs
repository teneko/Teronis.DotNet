using System;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SynchronizingCollection<SubItemType, SuperItemType> 
    {
        public class CollectionSynchronisationMirror<ToBeImitatedCollectionType>
            where ToBeImitatedCollectionType : INotifyCollectionSynchronizing<SuperItemType>, INotifyCollectionModified<SuperItemType>, INotifyCollectionSynchronized<SuperItemType>
        {
            private readonly SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection;

            public CollectionSynchronisationMirror(SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection, ToBeImitatedCollectionType toBeImitatedCollection)
            {
                toBeImitatedCollection.CollectionSynchronizing += ToBeImitatedCollection_CollectionSynchronizing;
                toBeImitatedCollection.CollectionModified += ToBeImitatedCollection_CollectionModified;
                toBeImitatedCollection.CollectionSynchronized += ToBeImitatedCollection_CollectionSynchronized;
                this.synchronizingCollection = synchronizingCollection;
            }

            private void ToBeImitatedCollection_CollectionModified(object sender, CollectionModifiedEventArgs<SuperItemType> e) =>
                synchronizingCollection.ApplyCollectionModification(e);

            private void ToBeImitatedCollection_CollectionSynchronizing(object sender, EventArgs e) =>
                synchronizingCollection.OnCollectionSynchronizing();

            private void ToBeImitatedCollection_CollectionSynchronized(object sender, EventArgs e) =>
                synchronizingCollection.OnCollectionSynchronized();
        }
    }
}
