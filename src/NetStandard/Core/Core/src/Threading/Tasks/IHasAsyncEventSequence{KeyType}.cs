

namespace Teronis.Threading.Tasks
{
    public interface IHasAsyncableEventSequence<KeyType>
        where KeyType : notnull
    {
        AsyncEventSequence<KeyType> AsyncEventSequence { get; }
    }
}
