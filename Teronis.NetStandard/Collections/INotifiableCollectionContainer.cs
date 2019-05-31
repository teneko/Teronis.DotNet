using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public interface INotifiableCollectionContainer<TItem> : INotifyCollectionChangeApplied<TItem>
    {
        IList<TItem> Collection { get; }
    }
}
