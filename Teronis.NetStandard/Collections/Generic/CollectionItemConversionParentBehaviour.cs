using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Teronis.Data;
using Teronis.Extensions.NetStandard;
using System.Linq;
using Teronis.Libraries.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionItemConversionParentBehaviour<OriginalItemType, OriginalContentType, ConvertedItemType>
        where OriginalItemType : IHaveKnownParents
    {
        public INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginalItemType, OriginalItemType> CollectionChangeConversionNotifer { get; private set; }

        public CollectionItemConversionParentBehaviour(INotifyCollectionChangeConversionApplied<ConvertedItemType, OriginalItemType, OriginalItemType> collectionChangeConversionNotifer)
        {
            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
        }

        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversionAppliedEventArgs<ConvertedItemType, OriginalItemType, OriginalItemType> args)
        {
            var convertedContentContentChange = args.ConvertedCollectionChangeBundle.ContentContentChange;
            var convertedItemItemChange = args.ConvertedCollectionChangeBundle.ItemItemChange;

            if (convertedContentContentChange.Action != convertedItemItemChange.Action)
                CollectionChangeConversionLibrary.ThrowActionMismatchException();

            var action = convertedContentContentChange.Action;

            switch (action) {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Add:
                    var originalItemsEnumerator = convertedContentContentChange.NewItems.GetEnumerator();
                    var convertedItemsEnumerator = convertedItemItemChange.NewItems.GetEnumerator();

                    while (originalItemsEnumerator.MoveNext() && convertedItemsEnumerator.MoveNext()) {
                        var originalItem = originalItemsEnumerator.Current;

                        switch (action) {
                            case NotifyCollectionChangedAction.Remove:
                                originalItem.DetachKnownWantParentsHandler(this);
                                break;
                            case NotifyCollectionChangedAction.Add:
                                var convertedItem = convertedItemsEnumerator.Current;

                                void OriginalItem_WantParents(object s, HavingParentsEventArgs e)
                                    => e.AttachParentParents(convertedItem);

                                originalItem.AttachKnownWantParentsHandler(this, OriginalItem_WantParents);
                                break;
                        }
                    }

                    break;
            }
        }
    }
}
