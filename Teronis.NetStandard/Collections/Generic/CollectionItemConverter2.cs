//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using Teronis.Extensions.NetStandard;
//using System.Threading.Tasks;

//namespace Teronis.Collections.Generic
//{
//    public abstract class CollectionItemConverter2<OriginalItemType, OriginalContentType, ConvertedItemType, ConvertedContentType> : INotifyCollectionChangeConversionApplied<OriginalItemType, OriginalContentType, ConvertedItemType>
//    {
//        public event EventHandler<object, CollectionChangeConversion<OriginalItemType, OriginalContentType, ConvertedItemType>> CollectionChangeConversionApplied;

//        public INotifyCollectionChangeApplied<OriginalItemType, OriginalContentType> OriginalCollectionChangeAppliedNotifier { get; private set; }
//        public IApplyCollectionChange<ConvertedItemType, ConvertedContentType> ConvertedCollectionChangeApplier { get; private set; }
//        public IReadOnlyList<ConvertedItemType> ConvertedItemCollection { get; private set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="originalCollectionChangeAppliedNotifier"></param>
//        /// <param name="convertedCollectionChangeApplier">The to be synchronized collection container which contains the boxed items.</param>
//        public CollectionItemConverter2(INotifyCollectionChangeApplied<OriginalItemType, OriginalContentType> originalCollectionChangeAppliedNotifier,
//            IApplyCollectionChange<ConvertedItemType, ConvertedContentType> convertedCollectionChangeApplier,
//            IReadOnlyList<ConvertedItemType> convertedItemCollection)
//        {
//            OriginalCollectionChangeAppliedNotifier = originalCollectionChangeAppliedNotifier ?? throw new ArgumentNullException(nameof(originalCollectionChangeAppliedNotifier));
//            OriginalCollectionChangeAppliedNotifier.CollectionChangeApplied += OriginalCollectionContainer_CollectionChangeApplied;
//            OriginalCollectionChangeAppliedNotifier.CollectionChangeApplied += OriginalCollectionContainer_CollectionChangeAppliedAsync;
//            ConvertedCollectionChangeApplier = convertedCollectionChangeApplier ?? throw new ArgumentNullException(nameof(convertedCollectionChangeApplier));
//            ConvertedItemCollection = convertedItemCollection;
//        }

//        public abstract ConvertedItemType ConvertOriginalCollectionItem(OriginalItemType item);

//        public virtual List<ConvertedItemType> ConvertOriginalCollectionItems(IEnumerable<OriginalItemType> items)
//        {
//            List<ConvertedItemType> list;

//            if (items is ICollection<OriginalItemType> collection)
//                list = new List<ConvertedItemType>(collection.Count);
//            else
//                list = new List<ConvertedItemType>();

//            foreach (var item in items)
//            {
//                var convertedItem = ConvertOriginalCollectionItem(item);
//                list.Add(convertedItem);
//            }

//            return list;
//        }

//        protected virtual CollectionChange<ConvertedItemType, ConvertedItemType> onConvertRemovingOriginalCollectionChange(ICollectionChange<OriginalItemType, OriginalItemType> change)
//        {
//            var oldConvertedItems = change.GetOldItems(ConvertedItemCollection, ConvertedItemCollection.Count);

//            return CollectionChange<ConvertedItemType, ConvertedItemType>.CreateOld(change.Action, oldConvertedItems, change.OldIndex);
//        }

//        protected virtual CollectionChange<ConvertedItemType, ConvertedItemType> onConvertAddingOriginalCollectionChange(ICollectionChange<OriginalItemType, OriginalItemType> change)
//        {
//            var newConvertedItems = ConvertOriginalCollectionItems(change.NewItems);
//            var convertedChange = CollectionChange<ConvertedItemType, ConvertedItemType>.CreateNew(change.Action, newConvertedItems, change.NewIndex);

//            return convertedChange;
//        }

//        protected virtual CollectionChange<ConvertedItemType, ConvertedItemType> onConvertMovingOriginalCollectionChange(ICollectionChange<OriginalItemType, OriginalItemType> change)
//        {
//            // Old items when moving are taken from the already converted items
//            var oldConvertedItems = change.GetOldItems(ConvertedItemCollection, ConvertedItemCollection.Count);
//            var newConvertedItems = ConvertOriginalCollectionItems(change.NewItems);

//            return new CollectionChange<ConvertedItemType, ConvertedItemType>(change.Action, oldConvertedItems, change.OldIndex, newConvertedItems, change.NewIndex);
//        }

//        protected virtual CollectionChange<ConvertedItemType, ConvertedItemType> onConvertReplacingOriginalCollectionChange(ICollectionChange<OriginalItemType, OriginalItemType> change)
//        {
//            var oldConvertedItems = change.GetOldItems(ConvertedItemCollection, ConvertedItemCollection.Count);
//            var newConvertedItems = ConvertOriginalCollectionItems(change.NewItems);
//            var convertedChange = new CollectionChange<ConvertedItemType, ConvertedItemType>(change.Action, oldConvertedItems, change.OldIndex, newConvertedItems, change.NewIndex);

//            return convertedChange;
//        }

//        protected virtual CollectionChange<ConvertedItemType, ConvertedItemType> onResettingtOriginalCollectionChange(ICollectionChange<OriginalItemType, OriginalItemType> change)
//        {
//            // TODO: aspect has been removed: look for alternative
//            var oldConvertedItems = ConvertedItemCollection;
//            //var newConvertedItems = ConvertOriginalCollectionItems(aspect.NewItems);
//            var newConvertedItems = ConvertOriginalCollectionItems(change.NewItems);
//            var convertedChange = new CollectionChange<ConvertedItemType, ConvertedItemType>(change.Action, oldConvertedItems, change.OldIndex, newConvertedItems, change.NewIndex);

//            return convertedChange;
//        }

//        public virtual CollectionChange<ConvertedItemType, ConvertedItemType> ConvertOriginalItemCollectionChange(IAspectedCollectionChange<OriginalItemType, OriginalContentType> aspectedOriginalContentChange, ICollectionChange<OriginalItemType, OriginalItemType> originalItemChange)
//        {
//            var change = originalItemChange;

//            switch (change.Action)
//            {
//                case NotifyCollectionChangedAction.Remove:
//                    return onConvertRemovingOriginalCollectionChange(change);
//                case NotifyCollectionChangedAction.Add:
//                    return onConvertAddingOriginalCollectionChange(change);
//                case NotifyCollectionChangedAction.Move:
//                    return onConvertMovingOriginalCollectionChange(change);
//                case NotifyCollectionChangedAction.Replace:
//                    return onConvertReplacingOriginalCollectionChange(change);
//                case NotifyCollectionChangedAction.Reset:
//                    return onResettingtOriginalCollectionChange(change);
//                default:
//                    throw new NotImplementedException();
//            }
//        }

//        //public void ApplyConvertedChange(ICollectionChange<ConvertedItemType, ConvertedContentType> convertedChange)
//        //    => ConvertedCollectionContainer.ApplyChange(convertedChange);

//        //public Task ApplyConvertedChangeAsync(ICollectionChange<ConvertedItemType, ConvertedContentType> convertedChange)
//        //    => ConvertedCollectionContainer.ApplyChangeAsync(convertedChange);

//        public void ApplyChange(IAspectedChangeBundle<OriginalItemType, OriginalContentType> aspectedChangeBundle)
//        {
//            var aspectedItemItemChange = aspectedChangeBundle.AspectedItemItemChange;
//            var aspectedItemContentChange = aspectedChangeBundle.AspectedItemContentChange;

//            var convertedChange = ConvertOriginalItemCollectionChange(aspectedChangeBundle.AspectedItemContentChange, aspectedItemItemChange.Change);
//            ConvertedCollectionChangeApplier.ApplyCollectionChange(convertedChange);

//            var changeConversion = new CollectionChangeConversion<OriginalItemType, OriginalContentType, ConvertedItemType>(aspectedItemItemChange, aspectedContentChange, convertedChange);
//            CollectionChangeConversionApplied?.Invoke(this, changeConversion);
//        }

//        public async Task ApplyChangeAsync(IAspectedCollectionChange<OriginalItemType, OriginalItemType> aspectedItemChange, IAspectedCollectionChange<OriginalItemType, OriginalContentType> aspectedContentChange)
//        {
//            var convertedChange = ConvertOriginalItemCollectionChange(aspectedContentChange, aspectedItemChange.Change);
//            ConvertedCollectionChangeApplier.ApplyCollectionChange(convertedChange);

//            var eventSequence = new AsyncableEventSequence();
//            var changeConversion = new CollectionChangeConversion<OriginalItemType, OriginalContentType, ConvertedItemType>(aspectedItemChange, aspectedContentChange, convertedChange, eventSequence);
//            CollectionChangeConversionApplied?.Invoke(this, changeConversion);
//            await eventSequence.FinishDependenciesAsync();
//        }

//        private void OriginalCollectionContainer_CollectionChangeApplied(object sender, CollectionChangeAppliedEventArgs<OriginalItemType, OriginalContentType> args)
//        {
//            if (args.IsAsyncEvent())
//                return;

//            ApplyChange(args.AspectedItemItemChange, args.AspectedItemContentChange);
//        }

//        private async void OriginalCollectionContainer_CollectionChangeAppliedAsync(object sender, CollectionChangeAppliedEventArgs<OriginalItemType, OriginalContentType> args)
//        {
//            var eventSequence = args.EventSequence;

//            if (eventSequence == null)
//                return;

//            var tcs = eventSequence.RegisterDependency();

//            try
//            {
//                await ApplyChangeAsync(args.AspectedItemItemChange, args.AspectedItemContentChange);
//                tcs.SetResult();
//            }
//            catch (Exception error)
//            {
//                tcs.SetException(error);
//            }
//        }
//    }
//}
