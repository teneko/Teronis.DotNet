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
    public class CollectionItemConversionParentBehaviour<TOriginalItem, TConvertedItem>
        where TOriginalItem : IHaveKnownParents
    {
        public INotifyCollectionChangeConversionApplied<TOriginalItem, TConvertedItem> CollectionChangeConversionNotifer { get; private set; }

        public CollectionItemConversionParentBehaviour(INotifyCollectionChangeConversionApplied<TOriginalItem, TConvertedItem> collectionChangeConversionNotifer)
        {
            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
        }

        private void forwardAttachParentsParents(AspectedCollectionChange<TOriginalItem> aspectedOriginalChange, CollectionChange<TConvertedItem> convertedChange)
        {
            var originalChange = aspectedOriginalChange.Change;

            if (originalChange.Action != convertedChange.Action)
                CollectionChangeConversionLibrary.ThrowActionMismatchException();

            var action = originalChange.Action;

            switch (action) {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Add:
                    var originalItemsEnumerator = originalChange.NewItems.GetEnumerator();
                    var convertedItemsEnumerator = convertedChange.NewItems.GetEnumerator();

                    while (originalItemsEnumerator.MoveNext() && convertedItemsEnumerator.MoveNext()) {
                        var originalItem = originalItemsEnumerator.Current;

                        switch (action) {
                            case NotifyCollectionChangedAction.Remove:
                                originalItem.DetachWantParentsHandler(this);
                                break;
                            case NotifyCollectionChangedAction.Add:
                                var convertedItem = convertedItemsEnumerator.Current;

                                void OriginalItem_WantParents(object s, HavingParentsEventArgs e)
                                    => e.AttachParentParents(convertedItem);

                                originalItem.AttachWantParentsHandler(this, OriginalItem_WantParents);
                                break;
                        }
                    }

                    break;
            }
        }

        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversion<TOriginalItem, TConvertedItem> args)
        {
            var aspectedOriginalChange = args.AppliedOriginalChange;
            var convertedChange = args.ConvertedChange;

            forwardAttachParentsParents(aspectedOriginalChange, convertedChange);
        }
    }
}
