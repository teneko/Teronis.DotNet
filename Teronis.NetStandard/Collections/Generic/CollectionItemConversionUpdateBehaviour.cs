using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Teronis.Data;
using Teronis.Extensions.NetStandard;
using System.Linq;
using Teronis.Libraries.NetStandard;
using System.Threading.Tasks;

namespace Teronis.Collections.Generic
{
    public class CollectionItemConversionUpdateBehaviour<TOriginalItem, TConvertedItem>
        where TOriginalItem : IUpdatableContainer<TOriginalItem>
        where TConvertedItem : IUpdatableContainer<TOriginalItem>
    {
        public INotifyCollectionChangeConversionApplied<TOriginalItem, TConvertedItem> CollectionChangeConversionNotifer { get; private set; }

        public CollectionItemConversionUpdateBehaviour(INotifyCollectionChangeConversionApplied<TOriginalItem, TConvertedItem> collectionChangeConversionNotifer)
        {
            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionAppliedAsync;
        }

        private IEnumerable<UpdateWithTargetContainer<TOriginalItem, TConvertedItem>> getOldConvertedItemUpdateContainerIterator(AspectedCollectionChange<TOriginalItem> aspectedOriginalChange, CollectionChange<TConvertedItem> convertedChange)
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
                            TOriginalItem originalItemReplacement;

                            if (aspectedOriginalChange.ReplaceAspect.ReferencedReplacedOldItemByIndexDictionary.ContainsKey(oldOriginalIndex))
                                originalItemReplacement = newOriginalItemsEnumerator.Current;
                            else
                                originalItemReplacement = oldOriginalItemsEnumerator.Current;

                            var oldConvertedItemUpdate = new Update<TOriginalItem>(originalItemReplacement, this);

                            var oldConvertedItemUpdateContainer = new UpdateWithTargetContainer<TOriginalItem, TConvertedItem>() {
                                Update = oldConvertedItemUpdate,
                                Target = oldConvertedItem
                            };

                            yield return oldConvertedItemUpdateContainer;
                        }

                        break;
                    }
            }
        }

        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversion<TOriginalItem, TConvertedItem> args)
        {
            if (args.EventSequence != null)
                return;

            var aspectedOriginalChange = args.AppliedOriginalChange;
            var convertedChange = args.ConvertedChange;

            foreach (var oldConvertedItemUpdateContainer in getOldConvertedItemUpdateContainerIterator(aspectedOriginalChange, convertedChange)) {
                var target = oldConvertedItemUpdateContainer.Target;
                var update = oldConvertedItemUpdateContainer.Update;

                target.UpdateContainerBy(update);
            }
        }

        private async void ConvertedCollectionChangeNotifer_CollectionChangeConversionAppliedAsync(object sender, CollectionChangeConversion<TOriginalItem, TConvertedItem> args)
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

                    await target.UpdateContainerByAsync(update);
                }

                tcs.SetResult();
            } catch (Exception error) {
                tcs.SetException(error);
            }
        }

        private void OriginalItem_Updating(object sender, UpdatingEventArgs<TOriginalItem> args)
            /// Is handled if already handled or <see cref="Update{T}.UpdateCreationSource"/> is not reference equals this
            => args.Handled = args.Handled || !ReferenceEquals(args.Update.UpdateCreationSource, this);
    }
}
