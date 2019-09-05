using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Collections
{
    public interface ICollectionChange<OldItemType, NewItemType>
    {
        NotifyCollectionChangedAction Action { get; }
        IReadOnlyList<OldItemType> OldItems { get; }
        int OldIndex { get; }
        IReadOnlyList<NewItemType> NewItems { get; }
        int NewIndex { get; }
    }
}
