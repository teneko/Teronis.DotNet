

namespace Teronis.Collections.CollectionChanging
{
    public interface ICollectionChangeBundle<ItemType, ContentType>
    {
        ICollectionChange<ItemType, ItemType> ItemItemChange { get; }
        ICollectionChange<ItemType, ContentType> ItemContentChange { get; }
        ICollectionChange<ContentType, ContentType> ContentContentChange { get; }
    }
}
