using System;
using Teronis.Threading.Tasks;

namespace Teronis.Collections
{
    public class CollectionChangeAppliedEventArgs<ItemType, ContentType> : EventArgs, ICollectionChangeBundle<ItemType, ContentType>, IHasAsyncEventSequence
    {
        public static CollectionChangeAppliedEventArgs<ItemType, ContentType> CreateAsynchronous(ICollectionChangeBundle<ItemType, ContentType> bundle, AsyncEventSequence eventSequence)
        {
            eventSequence = eventSequence ?? throw new ArgumentNullException(nameof(eventSequence));
            return new CollectionChangeAppliedEventArgs<ItemType, ContentType>(bundle, eventSequence);
        }

        public static CollectionChangeAppliedEventArgs<ItemType, ContentType> CreateSynchronous(ICollectionChangeBundle<ItemType, ContentType> bundle)
            => new CollectionChangeAppliedEventArgs<ItemType, ContentType>(bundle, null);

        /// <summary>
        /// A value of not null means, that this instance is coming from an async event invocation.
        /// </summary>
        public AsyncEventSequence AsyncEventSequence { get; private set; }

        AsyncEventSequence<Singleton> IHasAsyncableEventSequence<Singleton>.AsyncEventSequence 
            => AsyncEventSequence;

        public ICollectionChange<ItemType, ItemType> ItemItemChange 
            => bundle.ItemItemChange;

        public ICollectionChange<ItemType, ContentType> ItemContentChange
            => bundle.ItemContentChange;

        public ICollectionChange<ContentType, ContentType> ContentContentChange
            => bundle.ContentContentChange;

        private ICollectionChangeBundle<ItemType, ContentType> bundle;

        private CollectionChangeAppliedEventArgs(ICollectionChangeBundle<ItemType, ContentType> bundle, AsyncEventSequence eventSequence)
        {
            this.bundle = bundle;
            AsyncEventSequence = eventSequence;
        }
    }
}
