using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeResetAspect<ItemType>
    {
        public ReadOnlyCollection<ItemType> NewItems { get; private set; }

        private List<ItemType> newItems;

        public CollectionChangeResetAspect()
        {
            newItems = new List<ItemType>();
            NewItems = new ReadOnlyCollection<ItemType>(NewItems);
        }

        public void SetNewItems(IEnumerable<ItemType> collection)
        {
            newItems.Clear();
            newItems.AddRange(collection);
        }
    }
}
