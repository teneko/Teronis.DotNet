using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface ISynchronizableCollectionContainer<ItemType> : ICanSynchronizeCollection<ItemType>
    {
        IList<ItemType> Collection { get; }

        void ApplyChange(CollectionChange<ItemType> change);
        Task ApplyChangeAsync(CollectionChange<ItemType> change);
    }
}
