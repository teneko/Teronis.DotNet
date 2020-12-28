namespace Teronis.Collections.Changes
{
    public interface IConvertedCollectionModificationBundle<out ConvertedItemType, out CommonValueType, out OriginContentType>
    {
        ICollectionModificationBundle<ConvertedItemType, CommonValueType> ConvertedBundle { get; }
        ICollectionModificationBundle<CommonValueType, OriginContentType> OriginBundle { get; }
    }
}
