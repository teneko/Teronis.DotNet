using System;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizationMirror<SuperItemType>
        where SuperItemType : notnull
    {
        private readonly ICollectionSynchronizationContext<SuperItemType> collection;

        internal SynchronizationMirror(ICollectionSynchronizationContext<SuperItemType> collection, ISynchronizedCollection<SuperItemType> toBeMirroredCollection)
        {
            toBeMirroredCollection.CollectionSynchronizing += ToBeMirroredCollection_CollectionSynchronizing;
            toBeMirroredCollection.CollectionModified += ToBeMirroredCollection_CollectionModified;
            toBeMirroredCollection.CollectionSynchronized += ToBeMirroredCollection_CollectionSynchronized;
            this.collection = collection;
        }

        private void ToBeMirroredCollection_CollectionSynchronizing(object? sender, EventArgs e) =>
            collection.BeginCollectionSynchronization();

        private void ToBeMirroredCollection_CollectionModified(object? sender, CollectionModifiedEventArgs<SuperItemType> e) =>
            collection.GoThroughModification(e);

        private void ToBeMirroredCollection_CollectionSynchronized(object? sender, EventArgs e) =>
            collection.EndCollectionSynchronization();
    }
}
