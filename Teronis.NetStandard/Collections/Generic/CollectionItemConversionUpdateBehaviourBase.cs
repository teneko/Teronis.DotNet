using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Data;
using Teronis.Libraries.NetStandard;

namespace Teronis.Collections.Generic
{
    public abstract class CollectionItemConversionUpdateBehaviourBase<OriginalItemType, ConvertedItemType, CommonItemType>
        where OriginalItemType : IUpdatableContent<CommonItemType, CommonItemType>
        where ConvertedItemType : IUpdatableContent<CommonItemType, CommonItemType>
    {
        public INotifyCollectionChangeConversionApplied<OriginalItemType, ConvertedItemType> CollectionChangeConversionNotifer { get; private set; }

        public CollectionItemConversionUpdateBehaviourBase(INotifyCollectionChangeConversionApplied<OriginalItemType, ConvertedItemType> collectionChangeConversionNotifer)
        {
            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionAppliedAsync;
        }

        protected abstract CommonItemType ConvertOriginalItem(OriginalItemType originalItem);

        private IEnumerable<UpdateWithTargetContainer<CommonItemType, ConvertedItemType>> getOldConvertedItemUpdateContainerIterator(AspectedCollectionChange<OriginalItemType> aspectedOriginalChange, CollectionChange<ConvertedItemType> convertedChange)
        {
            var originalChange = aspectedOriginalChange.Change;

            if (originalChange.Action != convertedChange.Action)
                CollectionChangeConversionLibrary.ThrowActionMismatchException();

            var action = originalChange.Action;

            switch (action) {
                case NotifyCollectionChangedAction.Remove: {
                        var oldConvertedItemsEnumerator = convertedChange.OldItems.GetEnumerator();

                        while (oldConvertedItemsEnumerator.MoveNext()) {
                            var oldConvertedItem = oldConvertedItemsEnumerator.Current;
                            oldConvertedItem.ContainerUpdating -= OriginalItem_Updating;
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Add: {
                        var newConvertedItemsEnumerator = convertedChange.NewItems.GetEnumerator();

                        while (newConvertedItemsEnumerator.MoveNext()) {
                            var newConvertedItem = newConvertedItemsEnumerator.Current;
                            newConvertedItem.ContainerUpdating += OriginalItem_Updating;
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Replace: {
                        var oldConvertedItemsEnumerator = convertedChange.OldItems.GetEnumerator();
                        var oldOriginalItemsEnumerator = originalChange.OldItems.GetEnumerator();
                        var newOriginalItemsEnumerator = originalChange.NewItems.GetEnumerator();
                        var oldOriginalIndex = originalChange.OldIndex;

                        while (oldConvertedItemsEnumerator.MoveNext() && oldOriginalItemsEnumerator.MoveNext() && newOriginalItemsEnumerator.MoveNext()) {
                            var oldConvertedItem = oldConvertedItemsEnumerator.Current;
                            OriginalItemType originalItemReplacement;

                            if (aspectedOriginalChange.ReplaceAspect.ReferencedReplacedOldItemByIndexDictionary.ContainsKey(oldOriginalIndex))
                                originalItemReplacement = newOriginalItemsEnumerator.Current;
                            else
                                originalItemReplacement = oldOriginalItemsEnumerator.Current;

                            var commonItemReplacement = ConvertOriginalItem(originalItemReplacement);
                            var oldConvertedItemUpdate = new Update<CommonItemType>(commonItemReplacement, this);

                            var oldConvertedItemUpdateContainer = new UpdateWithTargetContainer<CommonItemType, ConvertedItemType>() {
                                Update = oldConvertedItemUpdate,
                                Target = oldConvertedItem
                            };

                            yield return oldConvertedItemUpdateContainer;
                        }

                        break;
                    }
            }
        }

        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversion<OriginalItemType, ConvertedItemType> args)
        {
            if (args.EventSequence != null)
                return;

            var aspectedOriginalChange = args.AppliedOriginalChange;
            var convertedChange = args.ConvertedChange;

            foreach (var oldConvertedItemUpdateContainer in getOldConvertedItemUpdateContainerIterator(aspectedOriginalChange, convertedChange)) {
                var target = oldConvertedItemUpdateContainer.Target;
                var update = oldConvertedItemUpdateContainer.Update;

                target.UpdateContentBy(update);
            }
        }

        private async void ConvertedCollectionChangeNotifer_CollectionChangeConversionAppliedAsync(object sender, CollectionChangeConversion<OriginalItemType, ConvertedItemType> args)
        {
            if (args.EventSequence == null)
                return;

            var aspectedOriginalChange = args.AppliedOriginalChange;
            var convertedChange = args.ConvertedChange;
            var tcs = args.EventSequence.RegisterDependency();

            try {
                foreach (var oldConvertedItemUpdateContainer in getOldConvertedItemUpdateContainerIterator(aspectedOriginalChange, convertedChange)) {
                    var target = oldConvertedItemUpdateContainer.Target;
                    var update = oldConvertedItemUpdateContainer.Update;

                    await target.UpdateContentByAsync(update);
                }

                tcs.SetResult();
            } catch (Exception error) {
                tcs.SetException(error);
            }
        }

        private void OriginalItem_Updating(object sender, IUpdatingEventArgs<CommonItemType> args)
            /// Is handled if already handled or <see cref="Update{T}.UpdateCreationSource"/> is not reference equals this
            => args.Handled = args.Handled || !ReferenceEquals(args.Update.UpdateCreationSource, this);
    }
}
