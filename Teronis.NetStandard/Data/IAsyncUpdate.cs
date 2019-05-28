using System.Threading.Tasks;

namespace Teronis.Data
{
    public interface IAsyncUpdate<T>
    {
        Task UpdateByAsync(T content);
    }
}
