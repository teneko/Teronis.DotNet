using System;
using Teronis.Threading.Tasks;

namespace Teronis.Collections.CollectionChanging
{
    public class CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType> : EventArgs
    {
        public static CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType> CreateAsynchronous(IConversionCollectionChangeBundles<ConvertedItemType, CommonValueType, object> bundles, AsyncEventSequence eventSequence)
        {
            eventSequence = eventSequence ?? throw new ArgumentNullException(nameof(eventSequence));
            return new CollectionChangeConversionAppliedEventArgs<ConvertedItemType, CommonValueType>(bundles, eventSequence);
        }

        /// <summary>
        /// This is the aspected original change that has been already applied.
        /// </summary>
        public ICollectionChangeBundle<ConvertedItemType, CommonValueType> ConvertedCollectionChangeBundle { get; private set; }
        public ICollectionChangeBundle<CommonValueType, object> OriginCollectionChangeBundle { get; private set; }
        public AsyncEventSequence EventSequence { get; private set; }

        private CollectionChangeConversionAppliedEventArgs(IConversionCollectionChangeBundles<ConvertedItemType, CommonValueType, object> bundles,
            AsyncEventSequence eventSequence)
        {
            bundles = bundles ?? throw new ArgumentNullException(nameof(bundles));
            ConvertedCollectionChangeBundle = bundles.ConvertedBundle;
            OriginCollectionChangeBundle = bundles.OriginBundle;
            EventSequence = eventSequence;
        }
    }
}
