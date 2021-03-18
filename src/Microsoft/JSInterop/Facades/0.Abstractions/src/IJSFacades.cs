using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacades : IAsyncDisposable, IJSFacadeResolver
    {
        ValueTask<IJSModule> CreateModuleAsync(string moduleNameOrPath);

        IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference);
        ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName);
        ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference objectReference, string objectName);
    }
}
