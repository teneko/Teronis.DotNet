using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeConversion<TOriginalChange, TConvertedChange>
    {
        public AspectedCollectionChange<TOriginalChange> AppliedOriginalChange { get; private set; }
        public CollectionChange<TConvertedChange> ConvertedChange { get; private set; }
        public AwaitableEventHandling AwaitableEventHandling { get; private set; }

        public CollectionChangeConversion(AspectedCollectionChange<TOriginalChange> aspectedChange, CollectionChange<TConvertedChange> convertedChange)
        {
            ConversionTaskCompletionSource = new TaskCompletionSource();
            AppliedOriginalChange = aspectedChange ?? throw new ArgumentNullException(nameof(aspectedChange));
            ConvertedChange = convertedChange ?? throw new ArgumentNullException(nameof(ConvertedChange));
        }
    }
}
