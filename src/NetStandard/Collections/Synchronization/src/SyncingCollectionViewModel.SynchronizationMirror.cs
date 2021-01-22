using System;

namespace Teronis.Collections.Synchronization
{
    public abstract partial class SyncingCollectionViewModel<SubItemType, SuperItemType> 
    {
        public class SynchronizationMirror<ToBeImitatedCollectionType>
            where ToBeImitatedCollectionType : INotifyCollectionSynchronizing<SuperItemType>, INotifyCollectionModified<SuperItemType>, INotifyCollectionSynchronized<SuperItemType>
        {
            private readonly SyncingCollectionViewModel<SubItemType, SuperItemType> collectionViewModel;

            public SynchronizationMirror(SyncingCollectionViewModel<SubItemType, SuperItemType> collectionViewModel, ToBeImitatedCollectionType toBeImitatedCollection)
            {
                toBeImitatedCollection.CollectionSynchronizing += ToBeImitatedCollection_CollectionSynchronizing;
                toBeImitatedCollection.CollectionModified += ToBeImitatedCollection_CollectionModified;
                toBeImitatedCollection.CollectionSynchronized += ToBeImitatedCollection_CollectionSynchronized;
                this.collectionViewModel = collectionViewModel;
            }

            private void ToBeImitatedCollection_CollectionSynchronizing(object sender, EventArgs e) =>
                collectionViewModel.OnCollectionSynchronizing();

            private void ToBeImitatedCollection_CollectionModified(object sender, CollectionModifiedEventArgs<SuperItemType> e) =>
                collectionViewModel.ApplyCollectionModification(e);

            private void ToBeImitatedCollection_CollectionSynchronized(object sender, EventArgs e) =>
                collectionViewModel.OnCollectionSynchronized();
        }
    }
}
