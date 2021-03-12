using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectReference
    {
        ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, object?[] arguments);
        ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] args);
        ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] args);

        ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, object?[] args);
        ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] args);
        ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] args);
    }
}
