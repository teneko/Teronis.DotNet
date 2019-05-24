

namespace Teronis.Data
{
    public interface IUpdatable<T>
    {
        void UpdateBy(T content);
    }
}
