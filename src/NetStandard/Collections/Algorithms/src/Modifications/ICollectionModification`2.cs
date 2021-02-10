using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionModification<out OldItemType, out NewItemType> : ICollectionModificationParameters
    {
        new int OldIndex { get; }
        ICollectionModificationPart<OldItemType, NewItemType, OldItemType, NewItemType> OldPart { get; }
        IReadOnlyList<OldItemType>? OldItems { get; }

        new int NewIndex { get; }
        ICollectionModificationPart<OldItemType, NewItemType, NewItemType, OldItemType> NewPart { get; }
        IReadOnlyList<NewItemType>? NewItems { get; }
    }
}
