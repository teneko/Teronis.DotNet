using System.Threading.Tasks;

namespace Teronis.Data
{
    public interface IAsyncUpdate<T> : IUpdatable<T>
    {
        Task UpdateByAsync(T content);
    }
}
