using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Data;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionSynchronizer<TList, TItem> : ISynchronizableCollectionContainer<TItem>, INotifiableCollectionContainer<TItem>, INotifyPropertyChanged, IContainerUpdateSequenceStatus
        where TList : IList<TItem>
    {
        public event CollectionChangeAppliedEventHandler<TItem> CollectionChangeApplied;
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsContainerUpdating => updateSequenceStatus.IsContainerUpdating;
        public TList Collection { get; private set; }
        public IEqualityComparer<TItem> EqualityComparer { get; private set; }

        private ContainerUpdateSequenceStatus updateSequenceStatus;
        private PropertyChangedRelay propertyChangedRelay;

        IList<TItem> ISynchronizableCollectionContainer<TItem>.Collection => Collection;
        IList<TItem> INotifiableCollectionContainer<TItem>.Collection => Collection;

        public CollectionSynchronizer(TList initialCollection, IEqualityComparer<TItem> equalityComparer)
        {
            updateSequenceStatus = new ContainerUpdateSequenceStatus();
            propertyChangedRelay = new PropertyChangedRelay(GetType(), updateSequenceStatus);
            propertyChangedRelay.PropertyChanged += PropertyChangedRelay_PropertyChanged;
            EqualityComparer = equalityComparer ?? EqualityComparer<TItem>.Default;
            Collection = initialCollection;
        }

        public CollectionSynchronizer(TList initialCollection)
            : this(initialCollection, default) { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(this, e);

        private void PropertyChangedRelay_PropertyChanged(object sender, PropertyChangedEventArgs e)
           => OnPropertyChanged(e);

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

        protected virtual AspectedCollectionChange<TItem> createAspectedCollectionChange(CollectionChange<TItem> change)
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

            return aspectedChange;
        }

        public void BeginContainerUpdate()
            => updateSequenceStatus.BeginContainerUpdate();

        public void EndContainerUpdate()
            => updateSequenceStatus.EndContainerUpdate();

        public virtual void ApplyChange(CollectionChange<TItem> change)
        {
            var aspectedChange = createAspectedCollectionChange(change);
            var args = new CollectionChangeAppliedEventArgs<TItem>(aspectedChange);
            CollectionChangeApplied?.Invoke(this, args);
        }

        public virtual async Task ApplyChangeAsync(CollectionChange<TItem> change)
        {
            var aspectedChange = createAspectedCollectionChange(change);
            var eventSequence = new AsyncableEventSequence();
            var args = new CollectionChangeAppliedEventArgs<TItem>(aspectedChange, eventSequence);
            CollectionChangeApplied?.Invoke(this, args);

            BeginContainerUpdate();
            await eventSequence.FinishDependenciesAsync();
            EndContainerUpdate();
        }

        private IEnumerable<CollectionChange<TItem>> getCollectionChanges(IEnumerable<TItem> items)
        {
            items = items ?? Enumerable.Empty<TItem>();

            //var cachedCollection = new List<TItem>(Collection);
            //var list = items.Take(5).ToList();
            //items = list.ReturnInValue((x) => x.Shuffle()).Take(ThreadSafeRandom.Next(0, list.Count + 1));

            var changes = Collection.GetCollectionChanges(items, EqualityComparer)
#if DEBUG
                .ToList()
#endif
            ;

            return changes;
        }

        public virtual void Synchronize(IEnumerable<TItem> items)
        {
            var changes = getCollectionChanges(items);

            foreach (var change in changes)
                ApplyChange(change);
        }

        public virtual async Task SynchronizeAsync(IEnumerable<TItem> items)
        {
            BeginContainerUpdate();
            var changes = getCollectionChanges(items);

            //try {
            foreach (var change in changes)
                await ApplyChangeAsync(change);
            //} catch {
            //    ;
            //}

            EndContainerUpdate();
        }

        public virtual async Task SynchronizeAsync(Task<IEnumerable<TItem>> itemsTask)
        {
            BeginContainerUpdate();
            var items = await itemsTask;
            await SynchronizeAsync(items);
            EndContainerUpdate();
        }
    }
}
