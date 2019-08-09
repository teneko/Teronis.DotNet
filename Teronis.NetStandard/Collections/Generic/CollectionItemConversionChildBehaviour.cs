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
    public class CollectionItemConversionChildBehaviour<TOriginalItem, TConvertedItem>
        where TConvertedItem : IHaveKnownParents
    {
        public INotifyCollectionChangeConversionApplied<TOriginalItem, TConvertedItem> CollectionChangeConversionNotifer { get; private set; }

        public CollectionItemConversionChildBehaviour(INotifyCollectionChangeConversionApplied<TOriginalItem, TConvertedItem> collectionChangeConversionNotifer)
        {
            CollectionChangeConversionNotifer = collectionChangeConversionNotifer;
            CollectionChangeConversionNotifer.CollectionChangeConversionApplied += ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied;
        }

        private void ConvertedCollectionChangeNotifer_CollectionChangeConversionApplied(object sender, CollectionChangeConversion<TOriginalItem, TConvertedItem> args)
        {
            var aspectedOriginalChange = args.AppliedOriginalChange;
            var originalChange = aspectedOriginalChange.Change;
            var convertedChange = args.ConvertedChange;

            if (originalChange.Action != convertedChange.Action)
                CollectionChangeConversionLibrary.ThrowActionMismatchException();

            var action = originalChange.Action;

            switch (action) {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Add:
                    var originalItemsEnumerator = originalChange.NewItems.GetEnumerator();
                    var convertedItemsEnumerator = convertedChange.NewItems.GetEnumerator();

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
