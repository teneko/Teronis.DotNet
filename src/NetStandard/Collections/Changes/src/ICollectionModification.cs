using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Changes
{
    public interface ICollectionModification<out OldItemType, out NewItemType>
    {
        NotifyCollectionChangedAction Action { get; }
        IReadOnlyList<OldItemType>? OldItems { get; }
        int OldIndex { get; }
        IReadOnlyList<NewItemType>? NewItems { get; }
        int NewIndex { get; }
    }
}
