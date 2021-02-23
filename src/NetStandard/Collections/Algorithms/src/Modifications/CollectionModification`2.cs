using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Teronis.Diagnostics;

namespace Teronis.Collections.Algorithms.Modifications
{
    [DebuggerDisplay(IDebuggerDisplayLibrary.FullGetDebuggerDisplayMethodPathWithParameterizedThis)]
    public class CollectionModification<NewItemType, OldItemType> : ICollectionModification<NewItemType, OldItemType>, IDebuggerDisplay, ICollectionModificationParameters
    {
        public static CollectionModification<NewItemType, OldItemType> CreateOld(NotifyCollectionChangedAction changeAction, IReadOnlyList<OldItemType>? oldItems, int oldIndex)
            => new CollectionModification<NewItemType, OldItemType>(changeAction, oldItems, oldIndex, null, -1);

        public static CollectionModification<NewItemType, OldItemType> CreateOld(NotifyCollectionChangedAction changeAction, [AllowNull] OldItemType oldItem, int oldIndex)
        {
            var oldItems = new OldItemType[] { oldItem! };
            return CreateOld(changeAction, oldItems, oldIndex);
        }

        public static CollectionModification<NewItemType, OldItemType> CreateNew(NotifyCollectionChangedAction changeAction, IReadOnlyList<NewItemType>? newValues, int newIndex)
            => new CollectionModification<NewItemType, OldItemType>(changeAction, null, -1, newValues, newIndex);

        public static CollectionModification<NewItemType, OldItemType> CreateNew(NotifyCollectionChangedAction changeAction, [AllowNull] NewItemType newItem, int newIndex)
        {
            var newItems = new NewItemType[] { newItem! };
            return CreateNew(changeAction, newItems, newIndex);
        }

        public NotifyCollectionChangedAction Action { get; private set; }
        public ICollectionModificationPart<NewItemType, OldItemType, OldItemType, NewItemType> OldPart => oldPart;
        public IReadOnlyList<OldItemType>? OldItems => oldPart.Items;
        int? ICollectionModificationParameters.OldItemsCount => oldPart.Items?.Count;

        public int OldIndex => oldPart.Index;
        public ICollectionModificationPart<NewItemType, OldItemType, NewItemType, OldItemType> NewPart => newPart;
        public IReadOnlyList<NewItemType>? NewItems => newPart.Items;
        int? ICollectionModificationParameters.NewItemsCount => newPart.Items?.Count;
        public int NewIndex => newPart.Index;

        string IDebuggerDisplay.DebuggerDisplay => $"{Action}, {nameof(OldIndex)} = {OldIndex}, {nameof(NewIndex)} = {NewIndex}";

        private readonly CollectionModificationPart<OldItemType, NewItemType> oldPart;
        private readonly CollectionModificationPart<NewItemType, OldItemType> newPart;

        public CollectionModification(NotifyCollectionChangedAction changeAction, IReadOnlyList<OldItemType>? oldItems, int oldIndex, IReadOnlyList<NewItemType>? newItems, int newIndex)
        {
            Action = changeAction;
            oldPart = new CollectionModificationPart<OldItemType, NewItemType>(this, PartialCollectionChangeItemState.OldItem, oldItems, oldIndex);
            newPart = new CollectionModificationPart<NewItemType, OldItemType>(this, PartialCollectionChangeItemState.NewItem, newItems, newIndex);
        }

        public CollectionModification(NotifyCollectionChangedAction changeAction, IReadOnlyList<OldItemType> oldValues, int oldIndex, NewItemType newItem, int newIndex)
            : this(changeAction, oldValues, oldIndex, new NewItemType[] { newItem }, newIndex) { }

        public CollectionModification(NotifyCollectionChangedAction changeAction, [AllowNull] OldItemType oldItem, int oldIndex, [AllowNull] NewItemType newItem, int newIndex)
            : this(changeAction, new OldItemType[] { oldItem! }, oldIndex, new NewItemType[] { newItem! }, newIndex) { }

        public CollectionModification(NotifyCollectionChangedAction changeAction, [AllowNull] OldItemType oldItem, int oldIndex, IReadOnlyList<NewItemType> newItems, int newIndex)
            : this(changeAction, new OldItemType[] { oldItem! }, oldIndex, newItems, newIndex) { }

        private enum PartialCollectionChangeItemState
        {
            OldItem,
            NewItem
        }

        public abstract class CollectionModificationPartBase<ItemType, OtherItemType> : ICollectionModificationPart<NewItemType, OldItemType, ItemType, OtherItemType>
        {
            public ICollectionModification<NewItemType, OldItemType> Owner { get; }

            public ICollectionModificationPart<NewItemType, OldItemType, OtherItemType, ItemType> OtherPart =>
                ReferenceEquals(this, Owner.OldPart) ? (ICollectionModificationPart<NewItemType, OldItemType, OtherItemType, ItemType>)Owner.NewPart :
                (ICollectionModificationPart<NewItemType, OldItemType, OtherItemType, ItemType>)Owner.OldPart;

            public abstract IReadOnlyList<ItemType>? Items { get; }
            public abstract int Index { get; }

            public CollectionModificationPartBase(ICollectionModification<NewItemType, OldItemType> modification)
            {
                Owner = modification;
            }
        }

        private class CollectionModificationPart<ItemType, OtherItemType> : CollectionModificationPartBase<ItemType, OtherItemType>
        {
            public PartialCollectionChangeItemState ItemState { get; private set; }
            public override IReadOnlyList<ItemType>? Items { get; }
            public override int Index { get; }

            public CollectionModificationPart(CollectionModification<NewItemType, OldItemType> modification,
                PartialCollectionChangeItemState itemState, IReadOnlyList<ItemType>? items, int index)
                : base(modification)
            {
                ItemState = itemState;
                Items = items;
                Index = index;
            }
        }
    }
}
