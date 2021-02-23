using System;

namespace Teronis.Collections.Synchronization
{
    public delegate void CollectionUpdateItemDelegate<ItemType, NewItemType>(ItemType item, Func<NewItemType> getNewItem);
}
