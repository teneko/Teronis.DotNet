using System;
using System.Collections.Specialized;
using Teronis.Collections.Changes;
using Teronis.ObjectModel.Parenting;

namespace Teronis.Collections.Synchronization
{
    public class CollectionItemConversionParentBehaviour<OriginalItemType, OriginalContentType, ConvertedItemType>
        where OriginalItemType : IHaveRegisteredParents
    {
        public INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginalItemType> CollectionChangeConversionNotifer { get; private set; }

        public CollectionItemConversionParentBehaviour(INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginalItemType> collectionChangeConversionNotifer)
        {
            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
        }

        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversionAppliedEventArgs<ConvertedItemType, OriginalItemType> args)
        {
            var convertedContentContentChange = args.ConvertedCollectionChangeBundle.OldSuperItemsNewSuperItemsModification;
            var convertedItemItemChange = args.ConvertedCollectionChangeBundle.OldSubItemsNewSubItemsModification;

            if (convertedContentContentChange.Action != convertedItemItemChange.Action) {
                CollectionChangeConversionThrowHelper.ThrowChangeActionMismatchException();
            }

            var action = convertedContentContentChange.Action;

            switch (action) {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Add:
                    var newConvertedContentContentItems = convertedContentContentChange.NewItems ??
                        throw new ArgumentException("No new converted content-content-items were given that can be attached as wanted parents.");

                    var newConvertedItemItemItems = convertedItemItemChange.NewItems ??
                        throw new ArgumentException("No new converted item-item-items were given that can be attached as wanted parents.");

                    var originalItemsEnumerator = newConvertedContentContentItems.GetEnumerator();
                    var convertedItemsEnumerator = newConvertedItemItemItems.GetEnumerator();

                    while (originalItemsEnumerator.MoveNext() && convertedItemsEnumerator.MoveNext()) {
                        var originalItem = originalItemsEnumerator.Current;

                        switch (action) {
                            case NotifyCollectionChangedAction.Remove:
                                originalItem.UnregisterParent(this);
                                break;
                            case NotifyCollectionChangedAction.Add:
                                var convertedItem = convertedItemsEnumerator.Current ??
                                    throw new ArgumentException("One item of the new converted item-item-items is null and cannot be attached as wanted parent.");

                                void OriginalItem_WantParents(object s, HavingParentsEventArgs e)
                                    => e.AddParentAndItsParents(convertedItem);

                                originalItem.RegisterParent(this, OriginalItem_WantParents);
                                break;
                        }
                    }

                    break;
            }
        }
    }
}
