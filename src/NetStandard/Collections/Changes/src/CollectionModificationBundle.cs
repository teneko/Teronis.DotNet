namespace Teronis.Collections.Changes
{
    public class CollectionModificationBundle<ItemType, ContentType> : ICollectionModificationBundle<ItemType, ContentType>
    {
        public ICollectionModification<ItemType, ItemType> OldSubItemsNewSubItemsModification { get; private set; }
        public ICollectionModification<ItemType, ContentType> OldSubItemsNewSuperItemsModification { get; private set; }
        public ICollectionModification<ContentType, ContentType> OldSuperItemsNewSuperItemsModification { get; private set; }

        public CollectionModificationBundle(ICollectionModification<ItemType, ItemType> itemItemChange,
            ICollectionModification<ItemType, ContentType> itemContentChange,
            ICollectionModification<ContentType, ContentType> contentContentChange)
        {
            OldSubItemsNewSubItemsModification = itemItemChange;
            OldSubItemsNewSuperItemsModification = itemContentChange;
            OldSuperItemsNewSuperItemsModification = contentContentChange;
        }
    }
}
