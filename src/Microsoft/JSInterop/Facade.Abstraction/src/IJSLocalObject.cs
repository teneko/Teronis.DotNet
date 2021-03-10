using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSLocalObject : IAsyncDisposable, IJSObjectReferenceFacade
    {
        ValueTask<IJSLocalObject> CreateObjectAsync(string objectName);
    }
}
