using System.Collections.Specialized;
using Teronis.Data;
using Teronis.Extensions;
using Teronis.Libraries.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionItemConversionChildBehaviour<OriginalItemType, OriginalContentType, ConvertedItemType>
        where ConvertedItemType : IHaveKnownParents
    {
        public INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginalItemType, OriginalContentType> CollectionChangeConversionNotifer { get; private set; }

        public CollectionItemConversionChildBehaviour(INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginalItemType, OriginalContentType> collectionChangeConversionNotifer)
        {
            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
        }

        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversionAppliedEventArgs<ConvertedItemType, OriginalItemType, OriginalContentType> args)
        {
            var convertedContentContentChange = args.ConvertedCollectionChangeBundle.ContentContentChange;
            var convertedItemItemChange = args.ConvertedCollectionChangeBundle.ItemItemChange;

            if (convertedContentContentChange.Action != convertedItemItemChange.Action)
                CollectionChangeConversionThrowHelper.ThrowChangeActionMismatchException();

            var action = convertedContentContentChange.Action;

            switch (action) {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Add:
                    var originalItemsEnumerator = convertedContentContentChange.NewItems.GetEnumerator();
                    var convertedItemsEnumerator = convertedItemItemChange.NewItems.GetEnumerator();

                    while (originalItemsEnumerator.MoveNext() && convertedItemsEnumerator.MoveNext()) {
                        var convertedItem = convertedItemsEnumerator.Current;

                        switch (action) {
                            case NotifyCollectionChangedAction.Remove:
                                convertedItem.DetachKnownWantParentsHandler(this);
                                break;
                            case NotifyCollectionChangedAction.Add:
                                var originalItem = originalItemsEnumerator.Current;

                                void OriginalItem_WantParents(object s, HavingParentsEventArgs e)
                                    => e.AttachParents(originalItem);

                                convertedItem.AttachKnownWantParentsHandler(this, OriginalItem_WantParents);
                                break;
                        }
                    }

                    break;
            }
        }
    }
}
