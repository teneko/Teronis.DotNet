using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeConversion<TOriginalChange, TConvertedChange>
    {
        public AspectedCollectionChange<TOriginalChange> AppliedOriginalChange { get; private set; }
        public CollectionChange<TConvertedChange> ConvertedChange { get; private set; }
        public AsyncableEventSequence EventSequence { get; private set; }

        public CollectionChangeConversion(AspectedCollectionChange<TOriginalChange> aspectedChange, CollectionChange<TConvertedChange> convertedChange, AsyncableEventSequence eventSequence)
        {
            AppliedOriginalChange = aspectedChange ?? throw new ArgumentNullException(nameof(aspectedChange));
            ConvertedChange = convertedChange ?? throw new ArgumentNullException(nameof(ConvertedChange));
            EventSequence = eventSequence;
        }

        public CollectionChangeConversion(AspectedCollectionChange<TOriginalChange> aspectedChange, CollectionChange<TConvertedChange> convertedChange)
            : this(aspectedChange, convertedChange, default) { }
    }
}
