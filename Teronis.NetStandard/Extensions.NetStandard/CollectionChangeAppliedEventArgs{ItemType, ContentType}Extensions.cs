using Teronis.Collections;

namespace Teronis.Extensions.NetStandard
{
    public static class CollectionChangeAppliedEventArgsExtensions
    {
        public static bool IsAsyncEvent<ItemType, ContentType>(this CollectionChangeAppliedEventArgs<ItemType, ContentType> args)
            => args.AsyncEventSequence != null;
    }
}
