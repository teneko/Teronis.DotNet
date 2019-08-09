using Teronis.Data;

namespace Teronis.Collections.Generic
{
    public class CollectionItemConversionUpdateBehaviour<OriginalItemType, ConvertedItemType> : CollectionItemConversionUpdateBehaviourBase<OriginalItemType, ConvertedItemType, OriginalItemType>
            where OriginalItemType : IUpdatableContent<OriginalItemType, OriginalItemType>
            where ConvertedItemType : IUpdatableContent<OriginalItemType, OriginalItemType>
    {
        public CollectionItemConversionUpdateBehaviour(INotifyCollectionChangeConversionApplied<OriginalItemType, ConvertedItemType> collectionChangeConversionNotifer)
        : base(collectionChangeConversionNotifer) { }

        protected override OriginalItemType ConvertOriginalItem(OriginalItemType originalItem)
            => originalItem;
    }
}
