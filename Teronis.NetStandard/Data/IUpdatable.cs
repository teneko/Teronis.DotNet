

namespace Teronis.Data
{
    public interface IUpdatable<T>
    {
        event UpdatingEventHandler<T> Updating;
        event UpdatedEventHandler<T> Updated;

        bool IsUpdating { get; }

        bool IsUpdatable(Update<T> update);
        void BeginUpdate();
        void UpdateBy(Update<T> update);
        void EndUpdate();
    }
}
