//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using Teronis.Data;
//using Teronis.Libraries.NetStandard;

//namespace Teronis.Collections.Generic
//{
//    public class CollectionItemConversionUpdateBehaviour<ConvertedItemType,OriginItemType, OriginContentType>
//        where ConvertedItemType : IUpdatableContent<OriginContentType>
//    {
//        public INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginItemType, OriginContentType> CollectionChangeConversionNotifer { get; private set; }

//        public CollectionItemConversionUpdateBehaviour(INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginItemType, OriginContentType> collectionChangeConversionNotifer)
//        {
//            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
//            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
//            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionAppliedAsync;
//        }

//        private void OriginalItem_Updating(object sender, IUpdatingEventArgs<OriginContentType> args)
//            /// Is handled if already handled or <see cref="Update{T}.UpdateCreationSource"/> is not reference equals this
//            => args.Handled = args.Handled || !ReferenceEquals(args.Update.UpdateCreationSource, this);

//        private IEnumerable<UpdateWithTargetContainer<OriginItemType, ConvertedItemType>> getOldConvertedItemUpdateContainerIterator(ICollectionChangeBundle<ConvertedItemType, OriginItemType> convertedBundle)
//        {
//            var convertedItemItemChange = convertedBundle.ItemItemChange;

//            switch (convertedItemItemChange.Action) {
//                case NotifyCollectionChangedAction.Remove: {
//                        var oldConvertedItemsEnumerator = convertedItemItemChange.OldItems.GetEnumerator();

//                        while (oldConvertedItemsEnumerator.MoveNext()) {
//                            var oldConvertedItem = oldConvertedItemsEnumerator.Current;
//                            oldConvertedItem.ContainerUpdating -= OriginalItem_Updating;
//                        }

//                        break;
//                    }
//                case NotifyCollectionChangedAction.Add: {
//                        var newConvertedItemsEnumerator = convertedItemItemChange.NewItems.GetEnumerator();

//                        while (newConvertedItemsEnumerator.MoveNext()) {
//                            var newConvertedItem = newConvertedItemsEnumerator.Current;
//                            newConvertedItem.ContainerUpdating += OriginalItem_Updating;
//                        }

//                        break;
//                    }
//                case NotifyCollectionChangedAction.Replace: {
//                        var convertedContentContentChange = convertedBundle.ContentContentChange;
//                        var oldConvertedItemEnumerator = convertedItemItemChange.OldItems.GetEnumerator();
//                        var newOriginItemEnumerator = convertedContentContentChange.NewItems.GetEnumerator();
//                        //var oldOriginalItemsEnumerator = originalItemChange.OldItems.GetEnumerator();
//                        //var oldOriginalIndex = originalItemChange.OldIndex;

//                        while (oldConvertedItemEnumerator.MoveNext() /*&& oldOriginalItemsEnumerator.MoveNext()*/ && newOriginItemEnumerator.MoveNext()) {
//                            var oldConvertedItem = oldConvertedItemEnumerator.Current;
//                            var commonValueReplacement = newOriginItemEnumerator.Current;

//                            var oldConvertedItemUpdate = new Update<OriginItemType>(commonValueReplacement, this);

//                            var oldConvertedItemUpdateContainer = new UpdateWithTargetContainer<OriginItemType, ConvertedItemType>() {
//                                Update = oldConvertedItemUpdate,
//                                Target = oldConvertedItem
//                            };

//                            yield return oldConvertedItemUpdateContainer;
//                        }

//                        break;
//                    }
//            }
//        }

//        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversion<ConvertedItemType, OriginItemType, OriginContentType> args)
//        {
//            if (args.EventSequence != null)
//                return;

//            //var aspectedOriginalItemChange = args.ConvertedCollectionChangeBundle;
//            //var aspectedOriginalContentChange = args.OriginCollectionChangeBundle;
//            //var convertedItemChange = args.ConvertedItemChange;

//            var convertedItemChange = args.ConvertedCollectionChangeBundle;

//            foreach (var oldConvertedItemUpdateContainer in getOldConvertedItemUpdateContainerIterator(convertedItemChange)) {
//                var target = oldConvertedItemUpdateContainer.Target;
//                var update = oldConvertedItemUpdateContainer.Update;

//                target.UpdateContentBy(update);
//            }
//        }

//        private async void ConvertedCollectionChangeNotifer_CollectionChangeConversionAppliedAsync(object sender, CollectionChangeConversion<ConvertedItemType, OriginItemType, OriginContentType> args)
//        {
//            if (args.EventSequence == null)
//                return;

//            var aspectedOriginalItemChange = args.ConvertedCollectionChangeBundle;
//            var aspectedOriginalContentChange = args.OriginCollectionChangeBundle;
//            var convertedItemChange = args.ConvertedItemChange;
//            var tcs = args.EventSequence.RegisterDependency();

//            try {
//                foreach (var oldConvertedItemUpdateContainer in getOldConvertedItemUpdateContainerIterator(aspectedOriginalItemChange, aspectedOriginalContentChange, convertedItemChange)) {
//                    var target = oldConvertedItemUpdateContainer.Target;
//                    var update = oldConvertedItemUpdateContainer.Update;

//                    await target.UpdateContentByAsync(update);
//                }

//                tcs.SetResult();
//            } catch (Exception error) {
//                tcs.SetException(error);
//            }
//        }
//    }
//}
