using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Teronis.Collections.ObjectModel;

namespace Teronis.Collections
{
    public class CollectionChange<T> : ICollectionChange<T>
    {
        public static CollectionChange<T> CreateLeft(NotifyCollectionChangedAction changeAction, T oldValue, int oldIndex)
            => new CollectionChange<T>(changeAction, oldValue, oldIndex, default, -1);

        public static CollectionChange<T> CreateLeft(NotifyCollectionChangedAction changeAction, T oldValue, int oldIndex, IndexShiftedNotifier indexShiftedNotifier)
            => new CollectionChange<T>(changeAction, oldValue, oldIndex, default, -1, indexShiftedNotifier);

        public static CollectionChange<T> CreateRight(NotifyCollectionChangedAction changeAction, T newValue, int newIndex)
            => new CollectionChange<T>(changeAction, default, -1, newValue, newIndex);

        public static CollectionChange<T> CreateRight(NotifyCollectionChangedAction changeAction, T newValue, int newIndex, IndexShiftedNotifier indexShiftedNotifier)
            => new CollectionChange<T>(changeAction, default, -1, newValue, newIndex, indexShiftedNotifier);

        public NotifyCollectionChangedAction ChangeAction { get; private set; }
        public T OldValue => oldPartialCollectionChange.Value;
        public int OldIndex => oldPartialCollectionChange.Index;
        public T NewValue => newPartialCollectionChange.Value;
        public int NewIndex => newPartialCollectionChange.Index;

        private PartialCollectionChange oldPartialCollectionChange, newPartialCollectionChange;

        public CollectionChange(NotifyCollectionChangedAction changeAction, T oldValue, int oldIndex, T newValue, int newIndex, IndexShiftedNotifier indexShiftedNotifier)
        {
            ChangeAction = changeAction;
            oldPartialCollectionChange = new PartialCollectionChange(PartialCollectionChangeItemState.OldItem, changeAction, oldValue, oldIndex, indexShiftedNotifier);
            newPartialCollectionChange = new PartialCollectionChange(PartialCollectionChangeItemState.NewItem, changeAction, newValue, newIndex, indexShiftedNotifier);
        }

        public CollectionChange(NotifyCollectionChangedAction changeAction, T oldValue, int oldIndex, T newValue, int newIndex)
            : this(changeAction, oldValue, oldIndex, newValue, newIndex, null) { }

        private enum PartialCollectionChangeItemState
        {
            OldItem,
            NewItem
        }

        private class PartialCollectionChange
        {
            public PartialCollectionChangeItemState ItemState { get; private set; }
            public NotifyCollectionChangedAction ChangeAction { get; private set; }
            public T Value { get; private set; }
            public int Index { get; private set; }
            public IndexShiftedNotifier IndexShiftedNotifier { get; private set; }

            public PartialCollectionChange(PartialCollectionChangeItemState itemState, NotifyCollectionChangedAction changeAction, T value, int index, IndexShiftedNotifier indexShiftedNotifier)
            {
                ItemState = itemState;
                ChangeAction = changeAction;
                Value = value;
                Index = index;
                IndexShiftedNotifier = indexShiftedNotifier;

                if (IndexShiftedNotifier != null && index != -1)
                    IndexShiftedNotifier.IndexShifted += LeftIndexShiftedNotifier_IndexShifted;
            }

            public PartialCollectionChange(PartialCollectionChangeItemState itemState, NotifyCollectionChangedAction changeAction, T value, int index)
                : this(itemState, changeAction, value, index, null) { }

            private void LeftIndexShiftedNotifier_IndexShifted(object sender, IndexShiftedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Remove && Index > e.Index)
                    Index--;
                else if (e.Action == NotifyCollectionChangedAction.Add && Index > e.Index)
                    Index++;
            }
        }
    }
}
