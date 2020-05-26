using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.CollectionChanging
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
