using Teronis.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class IHasAsyncableEventSequenceExtensions
    {
        public static bool IsAsyncEvent<KeyType>(this IHasAsyncableEventSequence<KeyType> asyncableEventSequenceContainer)
            where KeyType : notnull
            => !(asyncableEventSequenceContainer.AsyncEventSequence is null);
    }
}
