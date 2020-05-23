using System.Threading.Tasks;

namespace Teronis.ObjectModel.Updates
{
    public interface IApplyContentUpdateBy<in ContentType>
    {
        Task ApplyContentUpdateByAsync(IContentUpdate<ContentType> update);
    }
}
