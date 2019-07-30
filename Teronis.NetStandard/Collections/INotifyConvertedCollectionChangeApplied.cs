using Teronis.Collections.ObjectModel;

namespace Teronis.Collections
{
    public interface INotifyCollectionChangeConversionApplied<TOriginalChange, TConvertedChange>
    {
        event EventHandler<object, CollectionChangeConversion<TOriginalChange, TConvertedChange>> CollectionChangeConversionApplied;
    }
}
