using System;
using Teronis.Threading.Tasks;

namespace Teronis.Collections
{
    public class CollectionChangeAppliedEventArgs<ItemType, ContentType> : EventArgs, ICollectionChangeBundle<ItemType, ContentType>, IHasAsyncEventSequence
    {
        /// <summary>
        /// A value of not null means, that this instance is coming from an async event invocation.
        /// </summary>
        public AsyncEventSequence AsyncEventSequence { get; }

        AsyncEventSequence<Singleton> IHasAsyncableEventSequence<Singleton>.AsyncEventSequence
            => AsyncEventSequence;

        public ICollectionChange<ItemType, ItemType> ItemItemChange
            => bundle.ItemItemChange;

        public ICollectionChange<ItemType, ContentType> ItemContentChange
            => bundle.ItemContentChange;

        public ICollectionChange<ContentType, ContentType> ContentContentChange
            => bundle.ContentContentChange;

        private ICollectionChangeBundle<ItemType, ContentType> bundle;

        public CollectionChangeAppliedEventArgs(ICollectionChangeBundle<ItemType, ContentType> bundle, AsyncEventSequence eventSequence)
        {
            this.bundle = bundle ?? throw new ArgumentNullException(nameof(bundle));
            AsyncEventSequence = eventSequence ?? throw new ArgumentNullException(nameof(eventSequence));
        }
    }
}
