using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Teronis.Diagnostics;

namespace Teronis.Collections
{
    [DebuggerDisplay(IDebuggerDisplayLibrary.FullGetDebuggerDisplayMethodPathWithParameterizedThis)]
    public class CollectionChange<OldItemType, NewItemType> : ICollectionChange<OldItemType, NewItemType>, IDebuggerDisplay
    {
        public static CollectionChange<OldItemType, NewItemType> CreateOld(NotifyCollectionChangedAction changeAction, IReadOnlyList<OldItemType> oldItems, int oldIndex)
            => new CollectionChange<OldItemType, NewItemType>(changeAction, oldItems, oldIndex, null, -1);

        public static CollectionChange<OldItemType, NewItemType> CreateOld(NotifyCollectionChangedAction changeAction, OldItemType oldItem, int oldIndex)
        {
            var oldItems = new OldItemType[] { oldItem };
            return CreateOld(changeAction, oldItems, oldIndex);
        }

        public static CollectionChange<OldItemType, NewItemType> CreateNew(NotifyCollectionChangedAction changeAction, IReadOnlyList<NewItemType> newValues, int newIndex)
            => new CollectionChange<OldItemType, NewItemType>(changeAction, null, -1, newValues, newIndex);

        public static CollectionChange<OldItemType, NewItemType> CreateNew(NotifyCollectionChangedAction changeAction, NewItemType newItem, int newIndex)
        {
            var newItems = new NewItemType[] { newItem };
            return CreateNew(changeAction, newItems, newIndex);
        }

        public NotifyCollectionChangedAction Action { get; private set; }
        public IReadOnlyList<OldItemType> OldItems => oldPartialCollectionChange.Values;
        public int OldIndex => oldPartialCollectionChange.Index;
        public IReadOnlyList<NewItemType> NewItems => newPartialCollectionChange.Values;
        public int NewIndex => newPartialCollectionChange.Index;

        string IDebuggerDisplay.DebuggerDisplay => $"{Action}, {nameof(OldIndex)} = {OldIndex}, {nameof(NewIndex)} = {NewIndex}";

        private PartialCollectionChange<OldItemType> oldPartialCollectionChange;
        private PartialCollectionChange<NewItemType> newPartialCollectionChange;

        public CollectionChange(NotifyCollectionChangedAction changeAction, IReadOnlyList<OldItemType> oldItems, int oldIndex, IReadOnlyList<NewItemType> newItems, int newIndex)
        {
            Action = changeAction;
            oldPartialCollectionChange = new PartialCollectionChange<OldItemType>(PartialCollectionChangeItemState.OldItem, oldItems, oldIndex);
            newPartialCollectionChange = new PartialCollectionChange<NewItemType>(PartialCollectionChangeItemState.NewItem, newItems, newIndex);
        }

        public CollectionChange(NotifyCollectionChangedAction changeAction, IReadOnlyList<OldItemType> oldValues, int oldIndex, NewItemType newItem, int newIndex)
            : this(changeAction, oldValues, oldIndex, new NewItemType[] { newItem }, newIndex) { }

        public CollectionChange(NotifyCollectionChangedAction changeAction, OldItemType oldItem, int oldIndex, NewItemType newItem, int newIndex)
            : this(changeAction, new OldItemType[] { oldItem }, oldIndex, new NewItemType[] { newItem }, newIndex) { }

        public CollectionChange(NotifyCollectionChangedAction changeAction, OldItemType oldItem, int oldIndex, IReadOnlyList<NewItemType> newItems, int newIndex)
            : this(changeAction, new OldItemType[] { oldItem }, oldIndex, newItems, newIndex) { }

        private enum PartialCollectionChangeItemState
        {
            OldItem,
            NewItem
        }

        private class PartialCollectionChange<ItemType>
        {
            public PartialCollectionChangeItemState ItemState { get; private set; }
            public IReadOnlyList<ItemType> Values { get; private set; }
            public int Index { get; private set; }

            public PartialCollectionChange(PartialCollectionChangeItemState itemState, IReadOnlyList<ItemType> values, int index)
            {
                ItemState = itemState;
                Values = values;
                Index = index;
            }
        }
    }
}
