namespace Teronis.Collections.CollectionChanging
{
    public interface INotifyCollectionChangeConversionApplied<ConvertedItemType, CommonValueType>
    {
        event EventHandler<object, CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType>> CollectionChangeConversionApplied;
    }
}
