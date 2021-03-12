using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    public interface IJSLocalObjectActivator
    {
        IJSLocalObject CreateLocalObject(IJSObjectReference objectReference);
        ValueTask<IJSLocalObject> CreateLocalObjectAsync(string objectName);
        ValueTask<IJSLocalObject> CreateLocalObjectAsync(IJSObjectReference objectReference, string objectName);
        ValueTask<IJSLocalObject> CreateLocalObjectAsync(IJSLocalObject objectReference, string objectName);
    }
}
