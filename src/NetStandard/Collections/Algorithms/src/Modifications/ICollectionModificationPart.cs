using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionModificationPart<out OldItemType, out NewItemType, out ItemType, out OtherItemType>
    {
        ICollectionModification<OldItemType, NewItemType> Owner { get; }
        ICollectionModificationPart<OldItemType, NewItemType, OtherItemType, ItemType> OtherPart { get; }
        IReadOnlyList<ItemType>? Items { get; }
        int Index { get; }
    }
}
