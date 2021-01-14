namespace Teronis.Collections.Changes
{
    public enum CollectionModificationsActions
    {
        Insert = 1,
        Remove = 2,
        Replace = 4,
        InsertReplace = Insert | Replace,
        All = Insert | Remove | Replace,
    }
}
