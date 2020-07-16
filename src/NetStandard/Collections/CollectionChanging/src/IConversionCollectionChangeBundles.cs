

namespace Teronis.Collections.CollectionChanging
{
    public interface IConversionCollectionChangeBundles<ConvertedItemType, CommonValueType, OriginContentType>
    {
        ICollectionChangeBundle<ConvertedItemType, CommonValueType> ConvertedBundle { get; }
        ICollectionChangeBundle<CommonValueType, OriginContentType> OriginBundle { get; }
    }
}
