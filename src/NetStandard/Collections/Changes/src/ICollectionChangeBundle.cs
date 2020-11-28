

namespace Teronis.Collections.Changes
{
    public interface ICollectionChangeBundle<out ItemType, out ContentType>
    {
        ICollectionChange<ItemType, ItemType> ItemItemChange { get; }
        ICollectionChange<ItemType, ContentType> ItemContentChange { get; }
        ICollectionChange<ContentType, ContentType> ContentContentChange { get; }
    }
}
