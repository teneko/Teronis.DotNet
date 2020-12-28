using System;

namespace Teronis.Collections.Changes
{
    public class CollectionModifiedEventArgs<SubItemType, SuperItemType> : EventArgs, ICollectionModificationBundle<SubItemType, SuperItemType>
    {
        public ICollectionModification<SubItemType, SubItemType> OldSubItemsNewSubItemsModification
            => modificationBundle.OldSubItemsNewSubItemsModification;

        public ICollectionModification<SubItemType, SuperItemType> OldSubItemsNewSuperItemsModification
            => modificationBundle.OldSubItemsNewSuperItemsModification;

        public ICollectionModification<SuperItemType, SuperItemType> OldSuperItemsNewSuperItemsModification
            => modificationBundle.OldSuperItemsNewSuperItemsModification;

        private readonly ICollectionModificationBundle<SubItemType, SuperItemType> modificationBundle;

        public CollectionModifiedEventArgs(ICollectionModificationBundle<SubItemType, SuperItemType> modificationBundle) =>
            this.modificationBundle = modificationBundle ?? throw new ArgumentNullException(nameof(modificationBundle));
    }
}
