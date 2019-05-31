using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
   public interface ISynchronizableCollectionContainer<TItem>
    {
        IList<TItem> Collection { get; }

        void ApplyChange(CollectionChange<TItem> change);
        void Synchronize(IEnumerable<TItem> collection);
    }
}
