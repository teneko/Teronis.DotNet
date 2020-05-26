

namespace Teronis.Collections.CollectionChanging
{
    public interface INotifyCollectionChangeConversionApplied<ConvertedItemType, CommonValueType, OriginContentType>
    {
        event EventHandler<object, CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType, OriginContentType>> CollectionChangeConversionApplied;
    }
}
