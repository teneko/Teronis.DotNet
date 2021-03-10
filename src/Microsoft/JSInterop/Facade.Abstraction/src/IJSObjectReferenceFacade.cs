using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSObjectReferenceFacade : IAsyncDisposable
    {
        IJSObjectReference JSObjectReference { get; }

        ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args);
    }
}
