// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Collections.ObjectModel;
using Teronis.Utils;

namespace Teronis.Collections.Synchronization
{
    public class CollectionModifiedEventArgs<ItemType> : NotifyCollectionChangedEventArgs, ICollectionModification<ItemType, ItemType>
    {
        public ICollectionModificationPart<ItemType, ItemType, ItemType, ItemType> OldPart { get; }
        public ICollectionModificationPart<ItemType, ItemType, ItemType, ItemType> NewPart { get; }

        private readonly ICollectionModification<ItemType, ItemType> collectionModification;

        #region ICollectionModification<ItemType, ItemType>

        IReadOnlyList<ItemType>? ICollectionModification<ItemType, ItemType>.OldItems =>
            collectionModification.OldItems;

        int ICollectionModification<ItemType, ItemType>.OldIndex =>
            collectionModification.OldIndex;

        IReadOnlyList<ItemType>? ICollectionModification<ItemType, ItemType>.NewItems =>
            collectionModification.NewItems;

        int ICollectionModification<ItemType, ItemType>.NewIndex =>
            collectionModification.NewIndex;

        #endregion

        #region ICollectionModificationParameters

        int? ICollectionModificationParameters.OldItemsCount =>
            OldPart.Items?.Count;

        int ICollectionModificationParameters.OldIndex =>
            OldPart.Index;

        int? ICollectionModificationParameters.NewItemsCount =>
            NewPart.Items?.Count;

        int ICollectionModificationParameters.NewIndex =>
            NewPart.Index;

        #endregion

        public CollectionModifiedEventArgs(ICollectionModification<ItemType, ItemType> collectionModification)
            : base(NotifyCollectionChangedAction.Reset)
        {
            OldPart = new CollectionModificationOldPart(this);
            NewPart = new CollectionModificationNewPart(this);

            var oldItemReadOnlyCollection = collectionModification.OldItems is null ? null : new ReadOnlyList<ItemType>(collectionModification.OldItems);
            var newItemReadOnlyCollection = collectionModification.NewItems is null ? null : new ReadOnlyList<ItemType>(collectionModification.NewItems);

            NotifyCollectionChangedEventArgsUtils.SetNotifyCollectionChangedEventArgsProperties(this, collectionModification.Action,
                oldItemReadOnlyCollection, OldStartingIndex, newItemReadOnlyCollection, NewStartingIndex);

            this.collectionModification = collectionModification;
        }

        private abstract class CollectionModificationPartBase : CollectionModification<ItemType, ItemType>.CollectionModificationPartBase<ItemType, ItemType>
        {
            protected readonly ICollectionModification<ItemType, ItemType> Modification;

            public CollectionModificationPartBase(ICollectionModification<ItemType, ItemType> modification)
                : base(modification) =>
                Modification = modification;
        }

        private class CollectionModificationOldPart : CollectionModificationPartBase
        {
            public CollectionModificationOldPart(ICollectionModification<ItemType, ItemType> modification)
                : base(modification) { }

            public override IReadOnlyList<ItemType>? Items => Modification.OldItems;
            public override int Index => Modification.OldIndex;
        }

        private class CollectionModificationNewPart : CollectionModificationPartBase
        {
            public CollectionModificationNewPart(ICollectionModification<ItemType, ItemType> modification)
                : base(modification) { }

            public override IReadOnlyList<ItemType>? Items => Modification.NewItems;
            public override int Index => Modification.NewIndex;
        }
    }
}
