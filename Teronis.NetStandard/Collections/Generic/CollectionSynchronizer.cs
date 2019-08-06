using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Teronis.Data;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionSynchronizer<TList, TItem> : ISynchronizableCollectionContainer<TItem>, INotifiableCollectionContainer<TItem>, INotifyPropertyChanged, IUpdateSequenceStatus
        where TList : IList<TItem>
    {
        public event CollectionChangeAppliedEventHandler<TItem> CollectionChangeApplied;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsUpdating => updateSequenceStatus.IsUpdating;
        public TList Collection { get; private set; }
        public IEqualityComparer<TItem> EqualityComparer { get; private set; }
        
        private UpdateSequenceStatus updateSequenceStatus;
        private PropertyChangedRelay propertyChangedRelay;

        IList<TItem> ISynchronizableCollectionContainer<TItem>.Collection => Collection;
        IList<TItem> INotifiableCollectionContainer<TItem>.Collection => Collection;

        public CollectionSynchronizer(TList initialCollection, IEqualityComparer<TItem> equalityComparer)
        {
            updateSequenceStatus = new UpdateSequenceStatus();
            propertyChangedRelay = new PropertyChangedRelay(GetType(), updateSequenceStatus);
            propertyChangedRelay.PropertyChanged += PropertyChangedRelay_PropertyChanged;
            EqualityComparer = equalityComparer ?? EqualityComparer<TItem>.Default;
            Collection = initialCollection;
        }

        public CollectionSynchronizer(TList initialCollection)
            : this(initialCollection, default) { }

        private void PropertyChangedRelay_PropertyChanged(object sender, PropertyChangedEventArgs e)
           => PropertyChanged?.Invoke(this, e);

        [Conditional("DEBUG")]
        private void debugChange(CollectionChange<TItem> change, string oldItemNameFromCollection, bool tryDisplayOldItems, string newItemNameFromCollection, bool tryDisplayNewItems)
        {
            Debug.WriteLine($"{GetType().Name}, {change.ToDebugString()}");
            Debug.Indent();

            void displayItem(ICollection<TItem> items, string itemCollectionName, int beginningIndex)
            {
                try {
                    Debug.WriteLine($"{itemCollectionName}, BeginningIndex = {beginningIndex}");
                    Debug.Indent();

                    foreach (var item in items)
                        Debug.WriteLine($"{beginningIndex++}: {item.ToDebugString()}");

                    Debug.Unindent();
                } catch { }
            }

            Debug.Indent();
            Debug.WriteLine("OldItem(s)-Related");
            Debug.Indent();

            if (oldItemNameFromCollection != null)
                displayItem(change.GetOldItems(Collection), oldItemNameFromCollection, change.OldIndex);

            if (tryDisplayOldItems)
                displayItem(change.OldItems, nameof(change.OldItems), change.OldIndex);

            Debug.Unindent();
            Debug.Unindent();

            Debug.Indent();
            Debug.WriteLine("NewItem(s)-Related");
            Debug.Indent();

            if (newItemNameFromCollection != null)
                displayItem(change.GetNewItems(Collection), newItemNameFromCollection, change.NewIndex);

            if (tryDisplayNewItems)
                displayItem(change.OldItems, nameof(change.NewItems), change.NewIndex);

            Debug.Unindent();
            Debug.Unindent();

            Debug.Unindent();
        }

        protected virtual void onCollectionItemRemove(CollectionChange<TItem> change)
        {
            var oldIndex = change.OldIndex;

            for (var index = oldIndex + change.OldItems.Count - 1; index >= oldIndex; index--) {
#if DEBUG
                if (EqualityComparer != EqualityComparer<TItem>.Default) {
                    var removingItem = Collection[index];
                    var oldItemIndex = change.OldItems.Count - (oldIndex + change.OldItems.Count - index - 1) - 1;
                    var oldItem = change.OldItems[oldItemIndex];

                    if (!EqualityComparer.Equals(removingItem, oldItem))
                        throw new Exception("Removing item is not equals old item that should be removed instead");
                }
#endif
                Collection.RemoveAt(index);
            }
        }

        protected virtual void onCollectionItemAdd(CollectionChange<TItem> change)
        {
            var newIndex = change.NewIndex;

            foreach (var newItem in change.NewItems)
                Collection.Insert(newIndex++, newItem);
        }

        [Conditional("DEBUG")]
        private void debugAfterCollectionMove(CollectionChange<TItem> change)
        {
            Debug.Indent();
            debugChange(change, null, false, "MovedItems", false);
            Debug.Unindent();
        }

        /// <summary>
        /// Does regards <see cref="ObservableCollection{T}.Move(int, int)"/>, otherwise 
        /// it fallbacks to <see cref="IListGenericExtensions.Move{T}(IList{T}, int, int)"/>
        /// </summary>
        /// <param name="change"></param>
        protected virtual void onCollectionItemMove(CollectionChange<TItem> change)
        {
            debugChange(change, "MovingItems", false, null, false);

            if (Collection is ObservableCollection<TItem> observableCollection)
                observableCollection.Move(change.OldIndex, change.NewIndex);
            else
                Collection.Move(change.OldIndex, change.NewIndex);

            debugAfterCollectionMove(change);
        }

        /// <summary>
        /// This method has no code inside and is ready for override.
        /// </summary>
        protected virtual void onCollectionItemReplace(CollectionChange<TItem> change, CollectionChangeReplaceAspect<TItem> aspect)
        { }

        protected virtual void onCollectionReset(CollectionChange<TItem> change, CollectionChangeResetAspect<TItem> aspect)
        {
            var newItems = change.NewItems ?? throw new ArgumentNullException(nameof(change.NewItems));

            Collection.Clear();

            if (Collection is List<TItem> list)
                list.AddRange(newItems);
            else
                foreach (var item in newItems)
                    Collection.Add(item);

            aspect.SetNewItems(newItems);
        }

        public virtual void ApplyChange(CollectionChange<TItem> change)
        {
            AspectedCollectionChange<TItem> aspectedChange = null;

            switch (change.Action) {
                case NotifyCollectionChangedAction.Remove:
                    onCollectionItemRemove(change);
                    break;
                case NotifyCollectionChangedAction.Add:
                    onCollectionItemAdd(change);
                    break;
                case NotifyCollectionChangedAction.Move:
                    onCollectionItemMove(change);
                    break;
                case NotifyCollectionChangedAction.Replace: {
                        var aspect = new CollectionChangeReplaceAspect<TItem>();
                        onCollectionItemReplace(change, aspect);
                        aspectedChange = new AspectedCollectionChange<TItem>(change, aspect);
                        break;
                    }
                case NotifyCollectionChangedAction.Reset: {
                        var aspect = new CollectionChangeResetAspect<TItem>();
                        onCollectionReset(change, aspect);
                        aspectedChange = new AspectedCollectionChange<TItem>(change, aspect);
                        break;
                    }
            }

            if (aspectedChange == null)
                aspectedChange = new AspectedCollectionChange<TItem>(change);

            CollectionChangeApplied?.Invoke(this, aspectedChange);
        }

        public void BeginUpdate()
            => updateSequenceStatus.BeginUpdate();

        public void EndUpdate()
            => updateSequenceStatus.EndUpdate();

        public virtual void Synchronize(IEnumerable<TItem> items)
        {
            updateSequenceStatus.BeginUpdate(true);
            items = items ?? Enumerable.Empty<TItem>();

            //var cachedCollection = new List<TItem>(Collection);
            //var list = items.Take(5).ToList();
            //items = list.ReturnInValue((x) => x.Shuffle()).Take(ThreadSafeRandom.Next(0, list.Count + 1));

            var changes = Collection.GetCollectionChanges(items, EqualityComparer)
#if DEBUG
                .ToList()
#endif
            ;

            //try {
            foreach (var change in changes)
                ApplyChange(change);
            //} catch {
            //    ;
            //}

            updateSequenceStatus.EndUpdate(true);

            //Debug.Assert(Collection.SequenceEqual(items, EqualityComparer), "The collection is not synchron with the new items");
            //var isSequenciallyEqual = Collection.SequenceEqual(items, EqualityComparer);
            //var cachedCollectionChanges = cachedCollection.GetCollectionChanges(items, EqualityComparer).ToList();
            //;
        }
    }
}
