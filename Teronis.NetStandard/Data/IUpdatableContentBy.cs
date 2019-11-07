using System.Threading.Tasks;

namespace Teronis.Data
{
    public interface IUpdatableContentBy<in ContentType>
    {
        void UpdateContentBy(IContentUpdate<ContentType> update);
        Task UpdateContentByAsync(IContentUpdate<ContentType> update);
    }
}
