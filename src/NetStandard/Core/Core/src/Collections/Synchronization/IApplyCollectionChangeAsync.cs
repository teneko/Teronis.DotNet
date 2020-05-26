using System.Threading.Tasks;
using Teronis.Collections.CollectionChanging;

namespace Teronis.Collections.Synchronization
{
    public interface IApplyCollectionChangeAsync<ItemType, ContentType>
    {
        Task ApplyCollectionChangeAsync(ICollectionChange<ContentType, ContentType> change);
        Task ApplyCollectionChangeAsync(ICollectionChange<ItemType, ContentType> change);
    }
}
