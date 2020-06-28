using Teronis.Collections.CollectionChanging;

namespace Teronis.Collections.Synchronization
{
    public interface IApplyCollectionChange<ItemType, ContentType>
    {
        void ApplyCollectionChange(ICollectionChange<ContentType, ContentType> change);
        void ApplyCollectionChange(ICollectionChange<ItemType, ContentType> change);
    }
}
