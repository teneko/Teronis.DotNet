using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeResetAspect<TItem>
    {
        public ReadOnlyCollection<TItem> NewItems { get; private set; }

        private List<TItem> newItems;

        public CollectionChangeResetAspect()
        {
            newItems = new List<TItem>();
            NewItems = new ReadOnlyCollection<TItem>(NewItems);
        }

        public void SetNewItems(IEnumerable<TItem> collection)
        {
            newItems.Clear();
            newItems.AddRange(collection);
        }
    }
}
