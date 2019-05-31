

namespace Teronis.Data
{
    public interface IUpdatable<T>
    {
        event UpdatingEventHandler<T> Updating;
        event UpdatedEventHandler<T> Updated;

        bool IsUpdatable(Update<T> update);
        void UpdateBy(Update<T> update);
    }
}
