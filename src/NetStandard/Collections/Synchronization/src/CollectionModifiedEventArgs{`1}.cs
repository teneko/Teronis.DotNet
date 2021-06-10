// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Collections.ObjectModel;

namespace Teronis.Collections.Synchronization
{
    public class CollectionModifiedEventArgs<TItem> : NotifyCollectionChangedEventArgs, ICollectionModification<TItem, TItem>
    {
        public ICollectionModificationPart<TItem, TItem, TItem, TItem> OldPart { get; }
        public ICollectionModificationPart<TItem, TItem, TItem, TItem> NewPart { get; }

        private readonly ICollectionModification<TItem, TItem> collectionModification;

        #region ICollectionModification<ItemType, ItemType>

        IReadOnlyList<TItem>? ICollectionModification<TItem, TItem>.OldItems =>
            collectionModification.OldItems;

        int ICollectionModification<TItem, TItem>.OldIndex =>
            collectionModification.OldIndex;

        IReadOnlyList<TItem>? ICollectionModification<TItem, TItem>.NewItems =>
            collectionModification.NewItems;

        int ICollectionModification<TItem, TItem>.NewIndex =>
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

        public CollectionModifiedEventArgs(ICollectionModification<TItem, TItem> collectionModification)
            : base(NotifyCollectionChangedAction.Reset)
        {
            OldPart = new CollectionModificationOldPart(this);
            NewPart = new CollectionModificationNewPart(this);

            var oldItemReadOnlyCollection = collectionModification.OldItems is null ? null : new ReadOnlyList<TItem>(collectionModification.OldItems);
            var newItemReadOnlyCollection = collectionModification.NewItems is null ? null : new ReadOnlyList<TItem>(collectionModification.NewItems);

            NotifyCollectionChangedEventArgsUtils.SetNotifyCollectionChangedEventArgsProperties(this, collectionModification.Action,
                oldItemReadOnlyCollection, OldStartingIndex, newItemReadOnlyCollection, NewStartingIndex);

            this.collectionModification = collectionModification;
        }

        private abstract class CollectionModificationPartBase : CollectionModification<TItem, TItem>.CollectionModificationPartBase<TItem, TItem>
        {
            protected readonly ICollectionModification<TItem, TItem> Modification;

            public CollectionModificationPartBase(ICollectionModification<TItem, TItem> modification)
                : base(modification) =>
                Modification = modification;
        }

        private class CollectionModificationOldPart : CollectionModificationPartBase
        {
            public CollectionModificationOldPart(ICollectionModification<TItem, TItem> modification)
                : base(modification) { }

            public override IReadOnlyList<TItem>? Items => Modification.OldItems;
            public override int Index => Modification.OldIndex;
        }

        private class CollectionModificationNewPart : CollectionModificationPartBase
        {
            public CollectionModificationNewPart(ICollectionModification<TItem, TItem> modification)
                : base(modification) { }

            public override IReadOnlyList<TItem>? Items => Modification.NewItems;
            public override int Index => Modification.NewIndex;
        }
    }
}
