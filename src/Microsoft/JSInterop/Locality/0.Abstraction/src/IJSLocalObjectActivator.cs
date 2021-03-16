using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public interface IJSLocalObjectActivator
    {
        IJSLocalObject CreateInstance(IJSObjectReference objectReference);
        ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName);
        ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference objectReference, string objectName);
        ValueTask<IJSLocalObject> CreateInstanceAsync(IJSLocalObject objectReference, string objectName);
    }
}
