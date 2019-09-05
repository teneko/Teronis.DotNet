using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType, OriginContentType> : EventArgs
    {
        public static CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType, OriginContentType> CreateAsynchronous(IConversionCollectionChangeBundles<ConvertedItemType, CommonValueType, OriginContentType> bundles, AsyncableEventSequence eventSequence)
        {
            eventSequence = eventSequence ?? throw new ArgumentNullException(nameof(eventSequence));
            return new CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType, OriginContentType>(bundles, eventSequence);
        }

        public static CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType, OriginContentType> CreateSynchronous(IConversionCollectionChangeBundles<ConvertedItemType, CommonValueType, OriginContentType> bundles)
            => new CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType, OriginContentType>(bundles, null);

        /// <summary>
        /// This is the aspected original change that has been already applied.
        /// </summary>
        public ICollectionChangeBundle<ConvertedItemType, CommonValueType> ConvertedCollectionChangeBundle { get; private set; }
        public ICollectionChangeBundle<CommonValueType, OriginContentType> OriginCollectionChangeBundle { get; private set; }
        public AsyncableEventSequence EventSequence { get; private set; }

        private CollectionChangeConversionAppliedEventArgs(IConversionCollectionChangeBundles<ConvertedItemType, CommonValueType, OriginContentType> bundles,
            AsyncableEventSequence eventSequence)
        {
            bundles = bundles ?? throw new ArgumentNullException(nameof(bundles));
            EventSequence = eventSequence;
        }
    }
}
