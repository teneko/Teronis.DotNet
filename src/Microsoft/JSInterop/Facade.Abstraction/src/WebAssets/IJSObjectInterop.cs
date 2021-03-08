using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.WebAssets
{
    public interface IJSObjectInterop
    {
        IJSLocalObjectReference CreateObjectReference(IJSObjectReference objectReference);

        ValueTask<IJSLocalObjectReference> CreateObjectReferenceAsync(IJSObjectReference objectReference, string objectName);
        ValueTask<IJSLocalObjectReference> CreateObjectReferenceAsync(string? objectName);
    }
}
