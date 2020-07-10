using System.Threading.Tasks;
using Teronis.Models;

namespace Teronis.Services
{
    public interface IAsyncDialogService
    {
        Task<bool?> ShowDialogAsync(IDialogModel dialogModel);
    }
}
