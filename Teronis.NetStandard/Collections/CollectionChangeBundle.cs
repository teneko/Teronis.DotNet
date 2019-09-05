using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeBundle<ItemType, ContentType> : ICollectionChangeBundle<ItemType, ContentType>
    {
        public ICollectionChange<ItemType, ItemType> ItemItemChange { get; private set; }
        public ICollectionChange<ItemType, ContentType> ItemContentChange { get; private set; }
        public ICollectionChange<ContentType, ContentType> ContentContentChange { get; private set; }

        public CollectionChangeBundle(ICollectionChange<ItemType, ItemType> itemItemChange,
            ICollectionChange<ItemType, ContentType> itemContentChange,
            ICollectionChange<ContentType, ContentType> contentContentChange)
        {
            ItemItemChange = itemItemChange;
            ItemContentChange = itemContentChange;
            ContentContentChange = contentContentChange;
        }
    }
}
