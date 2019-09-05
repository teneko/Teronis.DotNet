using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Collections
{
    public class CollectionChangeAppliedEventArgs<ItemType, ContentType> : EventArgs, ICollectionChangeBundle<ItemType, ContentType>
    {
        public static CollectionChangeAppliedEventArgs<ItemType, ContentType> CreateAsynchronous(ICollectionChangeBundle<ItemType, ContentType> bundle, AsyncableEventSequence eventSequence)
        {
            eventSequence = eventSequence ?? throw new ArgumentNullException(nameof(eventSequence));
            return new CollectionChangeAppliedEventArgs<ItemType, ContentType>(bundle, eventSequence);
        }

        public static CollectionChangeAppliedEventArgs<ItemType, ContentType> CreateSynchronous(ICollectionChangeBundle<ItemType, ContentType> bundle)
            => new CollectionChangeAppliedEventArgs<ItemType, ContentType>(bundle, null);

        /// <summary>
        /// A value of not null means, that this instance is coming from an async event invocation.
        /// </summary>
        public AsyncableEventSequence EventSequence { get; private set; }

        public ICollectionChange<ItemType, ItemType> ItemItemChange 
            => bundle.ItemItemChange;

        public ICollectionChange<ItemType, ContentType> ItemContentChange
            => bundle.ItemContentChange;

        public ICollectionChange<ContentType, ContentType> ContentContentChange
            => bundle.ContentContentChange;

        private ICollectionChangeBundle<ItemType, ContentType> bundle;

        private CollectionChangeAppliedEventArgs(ICollectionChangeBundle<ItemType, ContentType> bundle, AsyncableEventSequence eventSequence)
        {
            this.bundle = bundle;
            EventSequence = eventSequence;
        }
    }
}
