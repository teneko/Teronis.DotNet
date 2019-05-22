using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Teronis.Collections.ObjectModel;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections
{
    public class CollectionChange<T> : ICollectionChange<T>
    {
        public static CollectionChange<T> CreateOld(NotifyCollectionChangedAction changeAction, T oldValue, int oldIndex)
            => new CollectionChange<T>(changeAction, oldValue, oldIndex, default, -1, -1);

        public static CollectionChange<T> CreateNew(NotifyCollectionChangedAction changeAction, T newValue, int newIndex, int originNewIndex)
            => new CollectionChange<T>(changeAction, default, -1, newValue, newIndex, originNewIndex);

        public NotifyCollectionChangedAction ChangeAction { get; private set; }
        public T OldValue => oldPartialCollectionChange.Value;
        public int OldIndex => oldPartialCollectionChange.Index;
        public T NewValue => newPartialCollectionChange.Value;
        public int NewIndex => newPartialCollectionChange.Index;
        public int OriginNewIndex { get; private set; }
        public bool IsSealed { get; private set; }

        private PartialCollectionChange oldPartialCollectionChange, newPartialCollectionChange;

        public CollectionChange(NotifyCollectionChangedAction changeAction, T oldValue, int oldIndex, T newValue, int newIndex, int originNewIndex)
        {
            ChangeAction = changeAction;
            OriginNewIndex = originNewIndex;

            oldPartialCollectionChange = new PartialCollectionChange(this, PartialCollectionChangeItemState.OldItem, oldValue, oldIndex);
            newPartialCollectionChange = new PartialCollectionChange(this, PartialCollectionChangeItemState.NewItem, newValue, newIndex);
        }

        private enum PartialCollectionChangeItemState
        {
            OldItem,
            NewItem
        }

        private class PartialCollectionChange
        {
            public PartialCollectionChangeItemState ItemState { get; private set; }
            public CollectionChange<T> Parent { get; private set; }
            public NotifyCollectionChangedAction ChangeAction => Parent.ChangeAction;
            public T Value { get; private set; }
            public int Index { get; private set; }
            public IndexShiftedNotifier IndexShiftedNotifier { get; private set; }

            public PartialCollectionChange(CollectionChange<T> parent, PartialCollectionChangeItemState itemState, T value, int index)
            {
                Parent = parent;
                ItemState = itemState;
                Value = value;
                Index = index;
            }
        }
    }
}
