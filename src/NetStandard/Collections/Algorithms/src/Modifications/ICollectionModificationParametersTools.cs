namespace Teronis.Collections.Algorithms.Modifications
{
    public static class ICollectionModificationParametersTools
    {
        public static bool MoveRangeContains(ICollectionModificationParameters modification, int index) =>
            CollectionTools.MoveRangeContains(modification.OldIndex, modification.NewIndex, modification.OldItemsCount!.Value, index);
    }
}
