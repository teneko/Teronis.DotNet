using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Changes;
using Teronis.Collections.Specialized;
using Teronis.Utils;

namespace Teronis.Collections.Synchronization
{
    public class NotifyCollectionModifiedEventArgs<ItemType> : NotifyCollectionChangedEventArgs, ICollectionModification<ItemType, ItemType>
    {
        private readonly ICollectionModification<ItemType, ItemType> collectionModification;

        public NotifyCollectionModifiedEventArgs(ICollectionModification<ItemType, ItemType> collectionModification)
            : base(NotifyCollectionChangedAction.Reset)
        {
            var oldItemReadOnlyCollection = collectionModification.OldItems is null ? null : new ReadOnlyList<ItemType>(collectionModification.OldItems);
            var newItemReadOnlyCollection = collectionModification.NewItems is null ? null : new ReadOnlyList<ItemType>(collectionModification.NewItems);

            NotifyCollectionChangedEventArgsUtils.SetNotifyCollectionChangedEventArgsProperties(this, collectionModification.Action,
                oldItemReadOnlyCollection, OldStartingIndex, newItemReadOnlyCollection, NewStartingIndex);

            this.collectionModification = collectionModification;
        }

        IReadOnlyList<ItemType>? ICollectionModification<ItemType, ItemType>.OldItems =>
            collectionModification.OldItems;

        int ICollectionModification<ItemType, ItemType>.OldIndex =>
            collectionModification.OldIndex;

        IReadOnlyList<ItemType>? ICollectionModification<ItemType, ItemType>.NewItems =>
            collectionModification.NewItems;

        int ICollectionModification<ItemType, ItemType>.NewIndex =>
            collectionModification.NewIndex;
    }
}
