using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Teronis.Collections
{
    public interface ISynchronizableCollectionContainer<TItem>
    {
        IList<TItem> Collection { get; }

        void ApplyChange(CollectionChange<TItem> change);
        Task ApplyChangeAsync(CollectionChange<TItem> change);
        void Synchronize(IEnumerable<TItem> collection);
        Task SynchronizeAsync(IEnumerable<TItem> collection);
        Task SynchronizeAsync(Task<IEnumerable<TItem>> collection);
    }
}
