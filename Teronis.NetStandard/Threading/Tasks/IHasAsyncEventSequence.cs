

namespace Teronis.Threading.Tasks
{
    public interface IHasAsyncEventSequence : IHasAsyncableEventSequence<Singleton>
    {
        new AsyncEventSequence AsyncEventSequence { get; }
    }
}
