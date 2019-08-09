using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MorseCode.ITask;
using Teronis.Data;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionSynchronizer<ListType, ItemType> : ISynchronizableCollectionContainer<ItemType>, INotifiableCollectionContainer<ItemType>, INotifyPropertyChanged, IContentUpdateSequenceStatus
        where ListType : IList<ItemType>
    {
        public event CollectionChangeAppliedEventHandler<ItemType> CollectionChangeApplied;
        public event PropertyChangedEventHandler PropertyChanged;
        public event WantParentsEventHandler WantParents;

        public virtual bool IsContentUpdating => updateSequenceStatus.IsContentUpdating;
        public ListType Collection { get; private set; }
        public IEqualityComparer<ItemType> EqualityComparer { get; private set; }

        private ContainerUpdateSequenceStatus updateSequenceStatus;
        private PropertyChangedRelay propertyChangedRelay;

        IList<ItemType> ISynchronizableCollectionContainer<ItemType>.Collection => Collection;
        IList<ItemType> INotifiableCollectionContainer<ItemType>.Collection => Collection;

        public CollectionSynchronizer(ListType initialCollection, IEqualityComparer<ItemType> equalityComparer)
        {
            updateSequenceStatus = new ContainerUpdateSequenceStatus();
            propertyChangedRelay = new PropertyChangedRelay(GetType(), updateSequenceStatus);
            propertyChangedRelay.PropertyChanged += PropertyChangedRelay_PropertyChanged;
            EqualityComparer = equalityComparer ?? EqualityComparer<ItemType>.Default;
            Collection = initialCollection;
        }

        public CollectionSynchronizer(ListType initialCollection)
            : this(initialCollection, default) { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(this, e);

        private void PropertyChangedRelay_PropertyChanged(object sender, PropertyChangedEventArgs e)
           => OnPropertyChanged(e);

        protected virtual void onCollectionItemRemove(CollectionChange<ItemType> change)
        {
            var oldIndex = change.OldIndex;

            for (var index = oldIndex + change.OldItems.Count - 1; index >= oldIndex; index--) {
#if DEBUG
                var removingItem = Collection[index];
                var oldItemIndex = change.OldItems.Count - (oldIndex + change.OldItems.Count - index - 1) - 1;
                var oldItem = change.OldItems[oldItemIndex];

                if (!EqualityComparer.Equals(removingItem, oldItem))
                    throw new Exception("Removing item is not equals old item that should be removed instead");
#endif
                Collection.RemoveAt(index);
            }
        }

        protected virtual void onCollectionItemAdd(CollectionChange<ItemType> change)
        {
            var newIndex = change.NewIndex;

            foreach (var newItem in change.NewItems)
                Collection.Insert(newIndex++, newItem);
        }

        /// <summary>
        /// Does regards <see cref="ObservableCollection{T}.Move(int, int)"/>, otherwise 
        /// it fallbacks to <see cref="IListGenericExtensions.Move{T}(IList{T}, int, int)"/>
        /// </summary>
        /// <param name="change"></param>
        protected virtual void onCollectionItemMove(CollectionChange<ItemType> change)
        {
            if (Collection is ObservableCollection<ItemType> observableCollection)
                observableCollection.Move(change.OldIndex, change.NewIndex);
            else
                Collection.Move(change.OldIndex, change.NewIndex);
        }

        /// <summary>
        /// This method has no code inside and is ready for override.
        /// </summary>
        protected virtual void onCollectionItemReplace(CollectionChange<ItemType> change, CollectionChangeReplaceAspect<ItemType> aspect)
        { }

        protected virtual void onCollectionReset(CollectionChange<ItemType> change, CollectionChangeResetAspect<ItemType> aspect)
        {
            var newItems = change.NewItems ?? throw new ArgumentNullException(nameof(change.NewItems));

            Collection.Clear();

            if (Collection is List<ItemType> list)
                list.AddRange(newItems);
            else
                foreach (var item in newItems)
                    Collection.Add(item);

            aspect.SetNewItems(newItems);
        }

        protected virtual AspectedCollectionChange<ItemType> createAspectedCollectionChange(CollectionChange<ItemType> change)
        {
            AspectedCollectionChange<ItemType> aspectedChange = null;

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
                        var aspect = new CollectionChangeReplaceAspect<ItemType>();
                        onCollectionItemReplace(change, aspect);
                        aspectedChange = new AspectedCollectionChange<ItemType>(change, aspect);
                        break;
                    }
                case NotifyCollectionChangedAction.Reset: {
                        var aspect = new CollectionChangeResetAspect<ItemType>();
                        onCollectionReset(change, aspect);
                        aspectedChange = new AspectedCollectionChange<ItemType>(change, aspect);
                        break;
                    }
            }

            if (aspectedChange == null)
                aspectedChange = new AspectedCollectionChange<ItemType>(change);

            return aspectedChange;
        }

        public void BeginContentUpdate()
            => updateSequenceStatus.BeginContentUpdate();

        public void EndContentUpdate()
            => updateSequenceStatus.EndContentUpdate();

        public virtual void ApplyChange(CollectionChange<ItemType> change)
        {
            var aspectedChange = createAspectedCollectionChange(change);
            var args = new CollectionChangeAppliedEventArgs<ItemType>(aspectedChange);
            CollectionChangeApplied?.Invoke(this, args);
        }

        public virtual async Task ApplyChangeAsync(CollectionChange<ItemType> change)
        {
            var aspectedChange = createAspectedCollectionChange(change);
            var eventSequence = new AsyncableEventSequence();
            var args = new CollectionChangeAppliedEventArgs<ItemType>(aspectedChange, eventSequence);
            CollectionChangeApplied?.Invoke(this, args);

            BeginContentUpdate();
            await eventSequence.FinishDependenciesAsync();
            EndContentUpdate();
        }

        private IEnumerable<CollectionChange<ItemType>> getCollectionChanges(IEnumerable<ItemType> items)
        {
            items = items ?? Enumerable.Empty<ItemType>();

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

        public virtual void Synchronize(IEnumerable<ItemType> items)
        {
            var changes = getCollectionChanges(items);

            foreach (var change in changes)
                ApplyChange(change);
        }

        public virtual async Task SynchronizeAsync(IEnumerable<ItemType> items)
        {
            BeginContentUpdate();
            var changes = getCollectionChanges(items);

            //try {
            foreach (var change in changes)
                await ApplyChangeAsync(change);
            //} catch {
            //    ;
            //}

            EndContentUpdate();
        }

        public virtual async Task SynchronizeAsync(ITask<IEnumerable<ItemType>> itemsTask)
        {
            BeginContentUpdate();
            var items = await itemsTask;
            await SynchronizeAsync(items);
            EndContentUpdate();
        }

        public ParentsPicker GetParentsPicker()
            => new ParentsPicker(this, WantParents);
    }
}
