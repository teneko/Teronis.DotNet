using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.ObjectModel;
using System.Diagnostics;
using Teronis.Diagnostics;

namespace Teronis.Collections
{
    [DebuggerDisplay("{debuggerDisplay}")]
    public class CollectionChange<T> : IDebuggerDisplay
    {
        public static CollectionChange<T> CreateOld(NotifyCollectionChangedAction changeAction, IList<T> oldItems, int oldIndex)
            => new CollectionChange<T>(changeAction, oldItems, oldIndex, null, -1);

        public static CollectionChange<T> CreateOld(NotifyCollectionChangedAction changeAction, T oldItem, int oldIndex)
        {
            var oldItems = new T[] { oldItem };
            return CreateOld(changeAction, oldItems, oldIndex);
        }

        public static CollectionChange<T> CreateNew(NotifyCollectionChangedAction changeAction, IList<T> newValues, int newIndex)
            => new CollectionChange<T>(changeAction, null, -1, newValues, newIndex);

        public static CollectionChange<T> CreateNew(NotifyCollectionChangedAction changeAction, T newItem, int newIndex)
        {
            var newItems = new T[] { newItem };
            return CreateNew(changeAction, newItems, newIndex);
        }

        public NotifyCollectionChangedAction Action { get; private set; }
        public IList<T> OldItems => oldPartialCollectionChange.Values;
        public int OldIndex => oldPartialCollectionChange.Index;
        public IList<T> NewItems => newPartialCollectionChange.Values;
        public int NewIndex => newPartialCollectionChange.Index;

        private string debuggerDisplay => ((IDebuggerDisplay)this).DebuggerDisplay;

        string IDebuggerDisplay.DebuggerDisplay => $"{Action}, {nameof(OldIndex)} = {OldIndex}, {nameof(NewIndex)} = {NewIndex}";

        private PartialCollectionChange oldPartialCollectionChange, newPartialCollectionChange;

        public CollectionChange(NotifyCollectionChangedAction changeAction, IList<T> oldItems, int oldIndex, IList<T> newItems, int newIndex)
        {
            Action = changeAction;
            oldPartialCollectionChange = new PartialCollectionChange(this, PartialCollectionChangeItemState.OldItem, oldItems, oldIndex);
            newPartialCollectionChange = new PartialCollectionChange(this, PartialCollectionChangeItemState.NewItem, newItems, newIndex);
        }

        public CollectionChange(NotifyCollectionChangedAction changeAction, IList<T> oldValues, int oldIndex, T newItem, int newIndex)
            : this(changeAction, oldValues, oldIndex, new T[] { newItem }, newIndex) { }

        public CollectionChange(NotifyCollectionChangedAction changeAction, T oldItem, int oldIndex, T newItem, int newIndex)
            : this(changeAction, new T[] { oldItem }, oldIndex, new T[] { newItem }, newIndex) { }

        public CollectionChange(NotifyCollectionChangedAction changeAction, T oldItem, int oldIndex, IList<T> newItems, int newIndex)
            : this(changeAction, new T[] { oldItem }, oldIndex, newItems, newIndex) { }

        private enum PartialCollectionChangeItemState
        {
            OldItem,
            NewItem
        }

        private class PartialCollectionChange
        {
            public PartialCollectionChangeItemState ItemState { get; private set; }
            public CollectionChange<T> Parent { get; private set; }
            public NotifyCollectionChangedAction ChangeAction => Parent.Action;
            public IList<T> Values { get; private set; }
            public int Index { get; private set; }

            public PartialCollectionChange(CollectionChange<T> parent, PartialCollectionChangeItemState itemState, IList<T> values, int index)
            {
                Parent = parent;
                ItemState = itemState;
                Values = values;
                Index = index;
            }
        }
    }
}
