using Teronis.Collections.ObjectModel;

namespace Teronis.Collections
{
    public interface INotifyCollectionChangeConversionApplied<ConvertedItemType, CommonValueType, OriginContentType>
    {
        event EventHandler<object, CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType, OriginContentType>> CollectionChangeConversionApplied;
    }
}
