using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public interface IJSLocalObjectInterop
    {
        ValueTask<IJSObjectReference> CreateObjectReferenceAsync(IJSObjectReference objectReference, string objectName);
        ValueTask<IJSObjectReference> CreateObjectReferenceAsync(string? objectName);
    }
}
