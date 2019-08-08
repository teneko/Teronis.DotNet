using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Extensions.NetStandard;
using System.Threading.Tasks;

namespace Teronis.Collections.Generic
{
    public abstract class CollectionItemConverter<TOriginalItem, TConvertedItem> : INotifyCollectionChangeConversionApplied<TOriginalItem, TConvertedItem>
    {
        public event EventHandler<object, CollectionChangeConversion<TOriginalItem, TConvertedItem>> CollectionChangeConversionApplied;

        public INotifiableCollectionContainer<TOriginalItem> OriginalCollectionChangeAppliedNotifier { get; private set; }
        public ISynchronizableCollectionContainer<TConvertedItem> ConvertedCollectionContainer { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalCollectionChangeAppliedNotifier"></param>
        /// <param name="convertedCollectionContainer">The to be synchronized collection container which contains the boxed items.</param>
        public CollectionItemConverter(INotifiableCollectionContainer<TOriginalItem> originalCollectionChangeAppliedNotifier, ISynchronizableCollectionContainer<TConvertedItem> convertedCollectionContainer)
        {
            OriginalCollectionChangeAppliedNotifier = originalCollectionChangeAppliedNotifier ?? throw new ArgumentNullException(nameof(originalCollectionChangeAppliedNotifier));
            OriginalCollectionChangeAppliedNotifier.CollectionChangeApplied += OriginalCollectionContainer_CollectionChangeApplied;
            OriginalCollectionChangeAppliedNotifier.CollectionChangeApplied += OriginalCollectionContainer_CollectionChangeAppliedAsync;
            ConvertedCollectionContainer = convertedCollectionContainer ?? throw new ArgumentNullException(nameof(convertedCollectionContainer));
        }

        public abstract TConvertedItem ConvertOriginalCollectionItem(TOriginalItem item);

        public virtual IList<TConvertedItem> ConvertOriginalCollectionItems(IEnumerable<TOriginalItem> items)
        {
            List<TConvertedItem> list;

            if (items is ICollection<TOriginalItem> collection)
                list = new List<TConvertedItem>(collection.Count);
            else
                list = new List<TConvertedItem>();

            foreach (var item in items) {
                var convertedItem = ConvertOriginalCollectionItem(item);
                list.Add(convertedItem);
            }

            return list;
        }

        protected virtual CollectionChange<TConvertedItem> onConvertRemovingOriginalCollectionChange(CollectionChange<TOriginalItem> change)
        {
            var oldConvertedItems = change.GetOldItems(ConvertedCollectionContainer.Collection);

            return CollectionChange<TConvertedItem>.CreateOld(change.Action, oldConvertedItems, change.OldIndex);
        }

        protected virtual CollectionChange<TConvertedItem> onConvertAddingOriginalCollectionChange(CollectionChange<TOriginalItem> change)
        {
            var newConvertedItems = ConvertOriginalCollectionItems(change.NewItems);
            var convertedChange = CollectionChange<TConvertedItem>.CreateNew(change.Action, newConvertedItems, change.NewIndex);

            return convertedChange;
        }

        protected virtual CollectionChange<TConvertedItem> onConvertMovingOriginalCollectionChange(CollectionChange<TOriginalItem> change)
        {
            // Old items when moving are taken from the already converted items
            var oldConvertedItems = change.GetOldItems(ConvertedCollectionContainer.Collection);
            var newConvertedItems = ConvertOriginalCollectionItems(change.NewItems);

            return new CollectionChange<TConvertedItem>(change.Action, oldConvertedItems, change.OldIndex, newConvertedItems, change.NewIndex);
        }

        protected virtual CollectionChange<TConvertedItem> onConvertReplacingOriginalCollectionChange(CollectionChange<TOriginalItem> change, CollectionChangeReplaceAspect<TOriginalItem> aspect)
        {
            var oldConvertedItems = change.GetOldItems(ConvertedCollectionContainer.Collection);
            var newConvertedItems = ConvertOriginalCollectionItems(change.NewItems);
            var convertedChange = new CollectionChange<TConvertedItem>(change.Action, oldConvertedItems, change.OldIndex, newConvertedItems, change.NewIndex);

            return convertedChange;
        }

        protected virtual CollectionChange<TConvertedItem> onResettingtOriginalCollectionChange(CollectionChange<TOriginalItem> change, CollectionChangeResetAspect<TOriginalItem> aspect)
        {
            var oldConvertedItems = ConvertedCollectionContainer.Collection;
            var newConvertedItems = ConvertOriginalCollectionItems(aspect.NewItems);
            var convertedChange = new CollectionChange<TConvertedItem>(change.Action, oldConvertedItems, change.OldIndex, newConvertedItems, change.NewIndex);

            return convertedChange;
        }

        public virtual CollectionChange<TConvertedItem> ConvertOriginalItemCollectionChange(AspectedCollectionChange<TOriginalItem> aspectedChange)
        {
            var change = aspectedChange.Change;

            switch (change.Action) {
                case NotifyCollectionChangedAction.Remove:
                    return onConvertRemovingOriginalCollectionChange(change);
                case NotifyCollectionChangedAction.Add:
                    return onConvertAddingOriginalCollectionChange(change);
                case NotifyCollectionChangedAction.Move:
                    return onConvertMovingOriginalCollectionChange(change);
                case NotifyCollectionChangedAction.Replace:
                    return onConvertReplacingOriginalCollectionChange(change, aspectedChange.ReplaceAspect);
                case NotifyCollectionChangedAction.Reset:
                    return onResettingtOriginalCollectionChange(change, aspectedChange.ResetAspect);
                default:
                    throw new NotImplementedException();
            }
        }

        public void ApplyChange(CollectionChange<TConvertedItem> convertedChange)
            => ConvertedCollectionContainer.ApplyChange(convertedChange);

        public Task ApplyChangeAsync(CollectionChange<TConvertedItem> convertedChange)
            => ConvertedCollectionContainer.ApplyChangeAsync(convertedChange);

        public void ApplyChange(AspectedCollectionChange<TOriginalItem> aspectedChange)
        {
            var convertedChange = ConvertOriginalItemCollectionChange(aspectedChange);
            ApplyChange(convertedChange);

            var changeConversion = new CollectionChangeConversion<TOriginalItem, TConvertedItem>(aspectedChange, convertedChange);
            CollectionChangeConversionApplied?.Invoke(this, changeConversion);
        }

        public async Task ApplyChangeAsync(AspectedCollectionChange<TOriginalItem> aspectedChange)
        {
            var convertedChange = ConvertOriginalItemCollectionChange(aspectedChange);
            await ApplyChangeAsync(convertedChange);

            var eventSequence = new AsyncableEventSequence();
            var changeConversion = new CollectionChangeConversion<TOriginalItem, TConvertedItem>(aspectedChange, convertedChange, eventSequence);
            CollectionChangeConversionApplied?.Invoke(this, changeConversion);
            await eventSequence.FinishDependenciesAsync();
        }

        private void OriginalCollectionContainer_CollectionChangeApplied(object sender, CollectionChangeAppliedEventArgs<TOriginalItem> args)
        {
            if (args.EventSequence != null)
                return;

            ApplyChange(args.AspectedCollectionChange);
        }

        private async void OriginalCollectionContainer_CollectionChangeAppliedAsync(object sender, CollectionChangeAppliedEventArgs<TOriginalItem> args)
        {
            var eventSequence = args.EventSequence;

            if (eventSequence == null)
                return;

            var tcs = eventSequence.RegisterDependency();

            try {
                await ApplyChangeAsync(args.AspectedCollectionChange);
                tcs.SetResult();
            } catch (Exception error) {
                tcs.SetException(error);
            }
        }
    }
}
