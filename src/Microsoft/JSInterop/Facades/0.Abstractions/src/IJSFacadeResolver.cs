using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadeResolver
    {
        ValueTask<IAsyncDisposable> CreateModuleFacadeAsync(string moduleNameOrPath, Type jsFacadeType);

        IAsyncDisposable CreateLocalObjectFacade(IJSObjectReference jsObjectReference, Type jsFacadeType);
        ValueTask<IAsyncDisposable> CreateLocalObjectFacadeAsync(string objectName, Type jsFacadeType);
        ValueTask<IAsyncDisposable> CreateLocalObjectFacadeAsync(IJSObjectReference jsObjectReference, string objectName, Type jsFacadeType);
    }
}
