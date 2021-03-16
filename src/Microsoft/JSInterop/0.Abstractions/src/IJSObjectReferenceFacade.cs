using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSObjectReferenceFacade
    {
        IJSObjectReference JSObjectReference { get; }

        ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] arguments);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[] arguments);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, object?[] arguments);

        ValueTask InvokeVoidAsync(string identifier, object?[] arguments);
        ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, object?[] arguments);
        ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, object?[] arguments);
    }
}
