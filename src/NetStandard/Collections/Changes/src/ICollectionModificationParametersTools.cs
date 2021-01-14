using Teronis.Tools;

namespace Teronis.Collections.Changes
{
    public static class ICollectionModificationParametersTools
    {
        public static bool MoveRangeContains(ICollectionModificationParameters modification, int index) =>
            CollectionTools.MoveRangeContains(modification.OldIndex, modification.NewIndex, modification.OldItemsCount!.Value, index);
    }
}
