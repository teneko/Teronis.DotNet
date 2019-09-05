﻿using System;
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
    public abstract class CollectionSynchronisation<ItemType, ContentType> : IApplyCollectionChange<ItemType, ContentType>, ICanSynchronizeCollection<ContentType>, INotifyCollectionChangeApplied<ItemType, ContentType>, INotifyPropertyChanged, IContentUpdateSequenceStatus
    {
        public event CollectionChangeAppliedEventHandler<ItemType, ContentType> CollectionChangeApplied;

        public event PropertyChangedEventHandler PropertyChanged;
        public event WantParentsEventHandler WantParents;

        public virtual bool IsContentUpdating => updateSequenceStatus.IsContentUpdating;
        public IList<ItemType> ItemList { get; private set; }
        public IList<ContentType> ContentList { get; private set; }
        public IEqualityComparer<ContentType> EqualityComparer { get; private set; }

        private ContainerUpdateSequenceStatus updateSequenceStatus;
        private PropertyChangedRelay propertyChangedRelay;

        public CollectionSynchronisation(IList<ItemType> initialItemCollection, IList<ContentType> initialContentCollection, IEqualityComparer<ContentType> equalityComparer)
        {
            updateSequenceStatus = new ContainerUpdateSequenceStatus();
            propertyChangedRelay = new PropertyChangedRelay(GetType(), updateSequenceStatus);
            propertyChangedRelay.NotifiersPropertyChanged += PropertyChangedRelay_NotifiersPropertyChanged;
            EqualityComparer = equalityComparer ?? EqualityComparer<ContentType>.Default;

            // Set lists
            ItemList = initialItemCollection;
            ContentList = initialContentCollection;

            CollectionChangeApplied += CollectionSynchronizer_CollectionChangeApplied;
        }

        public void BeginContentUpdate()
            => updateSequenceStatus.BeginContentUpdate();

        public void EndContentUpdate()
            => updateSequenceStatus.EndContentUpdate();

        private async void CollectionSynchronizer_CollectionChangeApplied(object sender, CollectionChangeAppliedEventArgs<ItemType, ContentType> args)
        {
            if (!args.IsAsyncEvent())
                return;

            var eventSequence = args.EventSequence;
            BeginContentUpdate();
            await eventSequence.TryAwaitDependency();
            EndContentUpdate();
        }

        public CollectionSynchronisation(IList<ItemType> initialItemCollection, IList<ContentType> initialContentCollection)
            : this(initialItemCollection, initialContentCollection, default) { }

        protected abstract ItemType CreateItem(ContentType content);

        public ConversionAdapter<OriginContentType> CreateConversionAdapter<OriginContentType>()
            => new ConversionAdapter<OriginContentType>(this);

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(this, e);

        private void PropertyChangedRelay_NotifiersPropertyChanged(object sender, PropertyChangedEventArgs e)
           => OnPropertyChanged(e);

        protected virtual void ApplyCollectionItemRemove(ApplyingCollectionChangeBundle bundle)
        {
            var change = bundle.ContentContentChange;
            var oldItemsCount = change.OldItems.Count;
            var oldIndex = change.OldIndex;

            for (var oldItemIndex = oldItemsCount - 1; oldItemIndex >= 0; oldItemIndex--)
            {
                var removeIndex = oldItemIndex + oldIndex;

#if DEBUG
                var removingContent = ContentList[removeIndex];
                var oldContent = change.OldItems[oldItemIndex];

                if (!EqualityComparer.Equals(removingContent, oldContent))
                    throw new Exception("Removing item is not equals old item that should be removed instead");
#endif
                ItemList.RemoveAt(removeIndex);
                ContentList.RemoveAt(removeIndex);
            }
        }

        protected virtual void ApplyCollectionItemAdd(ApplyingCollectionChangeBundle bundle)
        {
            var change = bundle.ContentContentChange;
            var newItemsCount = change.NewItems.Count;

            for (int itemIndex = 0; itemIndex < newItemsCount; itemIndex++)
            {
                var content = bundle.ItemContentChange.NewItems[itemIndex];
                var itemInsertIndex = change.NewIndex + itemIndex;
                var item = CreateItem(content);

                ItemList.Insert(itemInsertIndex, item);
                ContentList.Insert(itemInsertIndex, content);
            }
        }

        /// <summary>
        /// Does regards <see cref="ObservableCollection{T}.Move(int, int)"/>, otherwise 
        /// it fallbacks to <see cref="IListGenericExtensions.Move{T}(IList{T}, int, int)"/>
        /// </summary>
        /// <param name="change"></param>
        protected virtual void ApplyCollectionItemMove(ApplyingCollectionChangeBundle bundle)
        {
            var change = bundle.ItemContentChange;

            void moveItem<T>(IList<T> list)
            {
                if (list is ObservableCollection<ItemType> observableList)
                    observableList.Move(change.OldIndex, change.NewIndex);
                else
                    list.Move(change.OldIndex, change.NewIndex);
            }

            moveItem(ItemList);
            moveItem(ContentList);
        }

        /// <summary>
        /// This method has no code inside and is ready for override.
        /// </summary>
        protected virtual void ApplyCollectionItemReplace(ApplyingCollectionChangeBundle bundle)
        { }

        protected virtual void ApplyCollectionReset(ApplyingCollectionChangeBundle bundle)
        {
            var change = bundle.ItemContentChange;

            var newContentList = change.NewItems
                ?? throw new ArgumentNullException(nameof(change.NewItems));

            var newItemList = newContentList
                .Select(x => CreateItem(x));

            void resetList<T>(IList<T> list, IEnumerable<T> newList)
            {
                list.Clear();

                if (list is List<T> typedList)
                    typedList.AddRange(newList);
                else
                    foreach (var item in newList)
                        list.Add(item);
            }

            resetList(ItemList, newItemList);
            resetList(ContentList, newContentList);
        }

        protected virtual void ApplyCollectionChangeBundle(ApplyingCollectionChangeBundle bundle)
        {
            switch (bundle.ContentContentChange.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    ApplyCollectionItemRemove(bundle);
                    break;
                case NotifyCollectionChangedAction.Add:
                    ApplyCollectionItemAdd(bundle);
                    break;
                case NotifyCollectionChangedAction.Move:
                    ApplyCollectionItemMove(bundle);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ApplyCollectionItemReplace(bundle);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ApplyCollectionReset(bundle);
                    break;
            }
        }

        protected AppliedCollectionChangeBundle GetAppliedCollectionChangeBundle(ApplyingCollectionChangeBundle applyingBundle)
        {
            var itemContentChange = applyingBundle.ItemContentChange;
            var newItemList = itemContentChange.GetNewItems(ItemList);
            var itemItemChange = itemContentChange.CreateOf(itemContentChange.OldItems, newItemList);
            var appliedBundle = new AppliedCollectionChangeBundle(itemItemChange, applyingBundle.ItemContentChange, applyingBundle.ContentContentChange);
            return appliedBundle;
        }

        protected void OnCollectionChangeApplied(CollectionChangeAppliedEventArgs<ItemType, ContentType> args)
            => CollectionChangeApplied?.Invoke(this, args);

        #region ICollectionChange<ItemType, ContentType>

        public ICollectionChange<ContentType, ContentType> CreateContentContentCollectionChange(ICollectionChange<ItemType, ContentType> itemContentChange)
        {
            var oldItems = itemContentChange.GetNewItems(ContentList);
            var contentContentChange = itemContentChange.CreateOf(oldItems, itemContentChange.NewItems);
            return contentContentChange;
        }

        private void applyCollectionChange(ICollectionChange<ItemType, ContentType> itemContentChange, out ICollectionChangeBundle<ItemType, ContentType> appliedBundle)
        {
            var contentContentChange = CreateContentContentCollectionChange(itemContentChange);
            var applyingBundle = new ApplyingCollectionChangeBundle(itemContentChange, contentContentChange);
            ApplyCollectionChangeBundle(applyingBundle);
            appliedBundle = GetAppliedCollectionChangeBundle(applyingBundle);
        }

        public virtual void ApplyCollectionChange(ICollectionChange<ItemType, ContentType> itemContentChange)
        {
            applyCollectionChange(itemContentChange, out var appliedBundle);
            var args = CollectionChangeAppliedEventArgs<ItemType, ContentType>.CreateSynchronous(appliedBundle);
            OnCollectionChangeApplied(args);
        }

        public virtual async Task ApplyCollectionChangeAsync(ICollectionChange<ItemType, ContentType> itemContentChange)
        {
            applyCollectionChange(itemContentChange, out var appliedBundle);
            var eventSequence = new AsyncableEventSequence();
            var args = CollectionChangeAppliedEventArgs<ItemType, ContentType>.CreateAsynchronous(appliedBundle, eventSequence);
            OnCollectionChangeApplied(args);
            await eventSequence.FinishDependenciesAsync();
        }

        #endregion

        #region ICollectionChange<ItemType, ContentType>

        public ICollectionChange<ItemType, ContentType> CreateItemContentCollectionChange(ICollectionChange<ContentType, ContentType> contentContentChange)
        {
            var oldItems = contentContentChange.GetOldItems(ItemList);
            var itemContentChange = contentContentChange.CreateOf(oldItems, contentContentChange.NewItems);
            return itemContentChange;
        }

        private void applyCollectionChange(ICollectionChange<ContentType, ContentType> contentContentChange, out ICollectionChangeBundle<ItemType, ContentType> appliedBundle)
        {
            var itemContentChange = CreateItemContentCollectionChange(contentContentChange);
            var applyingBundle = new ApplyingCollectionChangeBundle(itemContentChange, contentContentChange);
            ApplyCollectionChangeBundle(applyingBundle);
            appliedBundle = GetAppliedCollectionChangeBundle(applyingBundle);
        }

        public virtual void ApplyCollectionChange(ICollectionChange<ContentType, ContentType> contentContentChange)
        {
            applyCollectionChange(contentContentChange, out var appliedBundle);
            var args = CollectionChangeAppliedEventArgs<ItemType, ContentType>.CreateSynchronous(appliedBundle);
            OnCollectionChangeApplied(args);
        }

        public virtual async Task ApplyCollectionChangeAsync(ICollectionChange<ContentType, ContentType> contentContentChange)
        {
            applyCollectionChange(contentContentChange, out var appliedBundle);
            var eventSequence = new AsyncableEventSequence();
            var args = CollectionChangeAppliedEventArgs<ItemType, ContentType>.CreateAsynchronous(appliedBundle, eventSequence);
            OnCollectionChangeApplied(args);
            await eventSequence.FinishDependenciesAsync();
        }

        #endregion

        private IEnumerable<CollectionChange<ContentType, ContentType>> getCollectionChanges(IEnumerable<ContentType> items)
        {
            items = items ?? Enumerable.Empty<ContentType>();

            //var cachedCollection = new List<TItem>(Collection);
            //var list = items.Take(5).ToList();
            //items = list.ReturnInValue((x) => x.Shuffle()).Take(ThreadSafeRandom.Next(0, list.Count + 1));

            var changes = ContentList.GetCollectionChanges(items, EqualityComparer)
#if DEBUG
                .ToList()
#endif
            ;

            return changes;
        }

        public virtual void Synchronize(IEnumerable<ContentType> items)
        {
            var changes = getCollectionChanges(items);

            foreach (ICollectionChange<ContentType, ContentType> change in changes)
                ApplyCollectionChange(change);
        }

        public virtual async Task SynchronizeAsync(IEnumerable<ContentType> items)
        {
            BeginContentUpdate();
            var changes = getCollectionChanges(items);

            foreach (var change in changes)
                await ApplyCollectionChangeAsync(change);

            EndContentUpdate();
        }

        public virtual async Task SynchronizeAsync(ITask<IEnumerable<ContentType>> itemsTask)
        {
            BeginContentUpdate();
            var items = await itemsTask;
            await SynchronizeAsync(items);
            EndContentUpdate();
        }

        public ParentsPicker GetParentsPicker()
            => new ParentsPicker(this, WantParents);

        protected class ApplyingCollectionChangeBundle
        {
            public ICollectionChange<ItemType, ContentType> ItemContentChange { get; private set; }
            public ICollectionChange<ContentType, ContentType> ContentContentChange { get; private set; }

            public ApplyingCollectionChangeBundle(ICollectionChange<ItemType, ContentType> itemContentChange,
                ICollectionChange<ContentType, ContentType> contentContentChange)
            {
                ItemContentChange = itemContentChange;
                ContentContentChange = contentContentChange;
            }
        }

        protected class AppliedCollectionChangeBundle : CollectionChangeBundle<ItemType, ContentType>
        {
            public AsyncableEventSequence EventSequence { get; private set; }

            public AppliedCollectionChangeBundle(ICollectionChange<ItemType, ItemType> itemItemChange,
            ICollectionChange<ItemType, ContentType> itemContentChange,
            ICollectionChange<ContentType, ContentType> contentContentChange,
            AsyncableEventSequence eventSequence)
                : base(itemItemChange, itemContentChange, contentContentChange)
                => EventSequence = eventSequence;

            public AppliedCollectionChangeBundle(ICollectionChange<ItemType, ItemType> itemItemChange,
            ICollectionChange<ItemType, ContentType> itemContentChange,
            ICollectionChange<ContentType, ContentType> contentContentChange)
                : this(itemItemChange, itemContentChange, contentContentChange, null)
            { }
        }

        public class ConversionAdapter<OriginContentType> : INotifyCollectionChangeConversionApplied<ItemType, ContentType, OriginContentType>
        {
            public event EventHandler<object, CollectionChangeConversionAppliedEventArgs<ItemType, ContentType, OriginContentType>> CollectionChangeConversionApplied;

            private CollectionSynchronisation<ItemType, ContentType> synchronizer;

            internal protected ConversionAdapter(CollectionSynchronisation<ItemType, ContentType> synchronizer)
                => this.synchronizer = synchronizer;

            protected void OnCollectionChangeConversionApplied(CollectionChangeConversionAppliedEventArgs<ItemType, ContentType, OriginContentType> args)
                => CollectionChangeConversionApplied?.Invoke(this, args);

            private ApplyingCollectionChangeBundle createApplyingCollectionChangeConversionBundle(ICollectionChangeBundle<ContentType, OriginContentType> originBundle)
            {

                var originContentContentChange = originBundle.ItemItemChange;
                var convertedItemContentChange = synchronizer.CreateItemContentCollectionChange(originContentContentChange);
                return new ApplyingCollectionChangeBundle(convertedItemContentChange, originContentContentChange);
            }

            public void RelayCollectionChange(ICollectionChangeBundle<ContentType, OriginContentType> originBundle)
            {
                var convertedApplyingBundle = createApplyingCollectionChangeConversionBundle(originBundle);
                synchronizer.ApplyCollectionChangeBundle(convertedApplyingBundle);
                var convertedAppliedBundle = synchronizer.GetAppliedCollectionChangeBundle(convertedApplyingBundle);
                var convertedChangeAppliedArgs = CollectionChangeAppliedEventArgs<ItemType, ContentType>.CreateSynchronous(convertedAppliedBundle);
                synchronizer.OnCollectionChangeApplied(convertedChangeAppliedArgs);

                var conversionBundles = new ConversionCollectionChangeBundles(convertedAppliedBundle, originBundle);
                var changeConversionAppliedArgs = CollectionChangeConversionAppliedEventArgs<ItemType, ContentType, OriginContentType>.CreateSynchronous(conversionBundles);
                OnCollectionChangeConversionApplied(changeConversionAppliedArgs);
            }

            public async Task RelayCollectionChangeAsync(ICollectionChangeBundle<ContentType, OriginContentType> originBundle)
            {
                var convertedApplyingBundle = createApplyingCollectionChangeConversionBundle(originBundle);
                synchronizer.ApplyCollectionChangeBundle(convertedApplyingBundle);
                var convertedAppliedBundle = synchronizer.GetAppliedCollectionChangeBundle(convertedApplyingBundle);
                var convertedChangeAppliedEventSequence = new AsyncableEventSequence();
                var convertedChangeAppliedArgs = CollectionChangeAppliedEventArgs<ItemType, ContentType>.CreateAsynchronous(convertedAppliedBundle, convertedChangeAppliedEventSequence);
                synchronizer.OnCollectionChangeApplied(convertedChangeAppliedArgs);
                await convertedChangeAppliedEventSequence.FinishDependenciesAsync();

                var conversionBundles = new ConversionCollectionChangeBundles(convertedAppliedBundle, originBundle);
                var changeConversionAppliedEventSequence = new AsyncableEventSequence();
                var changeConversionAppliedArgs = CollectionChangeConversionAppliedEventArgs<ItemType, ContentType, OriginContentType>.CreateAsynchronous(conversionBundles, changeConversionAppliedEventSequence);
                OnCollectionChangeConversionApplied(changeConversionAppliedArgs);
                await convertedChangeAppliedEventSequence.FinishDependenciesAsync();
            }

            protected class ConversionCollectionChangeBundles : IConversionCollectionChangeBundles<ItemType, ContentType, OriginContentType>
            {
                public ICollectionChangeBundle<ItemType, ContentType> ConvertedBundle { get; private set; }
                public ICollectionChangeBundle<ContentType, OriginContentType> OriginBundle { get; private set; }

                public ConversionCollectionChangeBundles(ICollectionChangeBundle<ItemType, ContentType> convertedBundle,
                    ICollectionChangeBundle<ContentType, OriginContentType> originBundle)
                {
                    ConvertedBundle = convertedBundle;
                    OriginBundle = originBundle;
                }
            }
        }
    }
}
