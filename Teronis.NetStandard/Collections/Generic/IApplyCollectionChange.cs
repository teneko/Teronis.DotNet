using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface IApplyCollectionChange<ItemType, ContentType>
    {
        void ApplyCollectionChange(ICollectionChange<ContentType, ContentType> change);
        Task ApplyCollectionChangeAsync(ICollectionChange<ContentType, ContentType> change);

        void ApplyCollectionChange(ICollectionChange<ItemType, ContentType> change);
        Task ApplyCollectionChangeAsync(ICollectionChange<ItemType, ContentType> change);
    }
}
