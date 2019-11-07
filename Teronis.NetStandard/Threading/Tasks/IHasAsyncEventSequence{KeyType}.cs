

namespace Teronis.Threading.Tasks
{
    public interface IHasAsyncableEventSequence<KeyType>
    {
        AsyncEventSequence<KeyType> AsyncEventSequence { get; }
    }
}
