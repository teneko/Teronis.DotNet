using System.Collections.Specialized;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionModificationParameters
    {
        NotifyCollectionChangedAction Action { get; }
        int? OldItemsCount { get; }
        int OldIndex { get; }
        int? NewItemsCount { get; }
        int NewIndex { get; }
    }
}
