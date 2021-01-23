using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionModification<out OldItemType, out NewItemType>
    {
        NotifyCollectionChangedAction Action { get; }
        ICollectionModificationPart<OldItemType, NewItemType, OldItemType, NewItemType> OldPart { get; }
        IReadOnlyList<OldItemType>? OldItems { get; }
        int OldIndex { get; }
        ICollectionModificationPart<OldItemType, NewItemType, NewItemType, OldItemType> NewPart { get; }
        IReadOnlyList<NewItemType>? NewItems { get; }
        int NewIndex { get; }
    }
}
