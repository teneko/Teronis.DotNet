using Teronis.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class IHasAsyncableEventSequenceExtensions
    {
        public static bool IsAsyncEvent<KeyType>(this IHasAsyncableEventSequence<KeyType> asyncableEventSequenceContainer)
            => !(asyncableEventSequenceContainer.AsyncEventSequence is null);
    }
}
