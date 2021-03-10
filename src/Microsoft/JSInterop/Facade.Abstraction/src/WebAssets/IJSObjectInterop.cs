using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.WebAssets
{
    public interface IJSObjectInterop
    {
        IJSLocalObject CreateObject(IJSObjectReference objectReference);
        ValueTask<IJSObjectReference> CreateObjectReferenceAsync(IJSObjectReference objectReference, string objectName);
        ValueTask<IJSObjectReference> CreateObjectReferenceAsync(string? objectName);
        ValueTask<IJSLocalObject> CreateObjectAsync(string objectName);
        ValueTask<IJSLocalObject> CreateObjectAsync(IJSObjectReference objectReference, string objectName);
        ValueTask<IJSLocalObject> CreateObjectAsync(IJSLocalObject objectReference, string objectName);
    }
}
