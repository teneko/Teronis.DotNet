// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizedCollectionMirror<TSuperItem>
        where TSuperItem : notnull
    {
        private readonly ICollectionSynchronizationContext<TSuperItem> collection;

        internal SynchronizedCollectionMirror(ICollectionSynchronizationContext<TSuperItem> collection, ISynchronizedCollection<TSuperItem> toBeMirroredCollection)
        {
            toBeMirroredCollection.CollectionSynchronizing += ToBeMirroredCollection_CollectionSynchronizing;
            toBeMirroredCollection.CollectionModified += ToBeMirroredCollection_CollectionModified;
            toBeMirroredCollection.CollectionSynchronized += ToBeMirroredCollection_CollectionSynchronized;
            this.collection = collection;
        }

        private void ToBeMirroredCollection_CollectionSynchronizing(object? sender, EventArgs e) =>
            collection.BeginCollectionSynchronization();

        private void ToBeMirroredCollection_CollectionModified(object? sender, CollectionModifiedEventArgs<TSuperItem> e) =>
            collection.ProcessModification(e);

        private void ToBeMirroredCollection_CollectionSynchronized(object? sender, EventArgs e) =>
            collection.EndCollectionSynchronization();
    }
}
