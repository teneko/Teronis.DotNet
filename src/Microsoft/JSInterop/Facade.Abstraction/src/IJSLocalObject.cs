using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSLocalObject : IAsyncDisposable, IJSObjectReferenceFacade
    {
        IJSObjectReference JSObjectReference { get; }
        ValueTask<IJSLocalObject> CreateObjectAsync(string objectName);
    }
}
