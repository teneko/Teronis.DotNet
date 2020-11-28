

namespace Teronis.Collections.Changes
{
    public interface IConversionCollectionChangeBundles<out ConvertedItemType, out CommonValueType, out OriginContentType>
    {
        ICollectionChangeBundle<ConvertedItemType, CommonValueType> ConvertedBundle { get; }
        ICollectionChangeBundle<CommonValueType, OriginContentType> OriginBundle { get; }
    }
}
