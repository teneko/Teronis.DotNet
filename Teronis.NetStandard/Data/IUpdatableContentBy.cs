using System.Threading.Tasks;
using MorseCode.ITask;

namespace Teronis.Data
{
    public interface IUpdatableContentBy<in ContentType>
    {
        void UpdateContentBy(IUpdate<ContentType> update);
        Task UpdateContentByAsync(IUpdate<ContentType> update);
        Task UpdateContentByAsync(ITask<IUpdate<ContentType>> updateTask);
    }
}
