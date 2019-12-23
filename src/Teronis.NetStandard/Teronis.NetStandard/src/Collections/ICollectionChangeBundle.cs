using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public interface ICollectionChangeBundle<ItemType, ContentType>
    {
        ICollectionChange<ItemType, ItemType> ItemItemChange { get; }
        ICollectionChange<ItemType, ContentType> ItemContentChange { get; }
        ICollectionChange<ContentType, ContentType> ContentContentChange { get; }
    }
}
