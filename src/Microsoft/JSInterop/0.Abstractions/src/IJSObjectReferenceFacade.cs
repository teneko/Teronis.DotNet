using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSObjectReferenceFacade
    {
        IJSObjectReference JSObjectReference { get; }

        ValueTask<TValue> InvokeAsync<TValue>(string identifier, [Accommodatable] object?[] arguments);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, [Cancellable] CancellationToken cancellationToken, [Accommodatable] object?[] args);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, [Cancellable] TimeSpan timeout, [Accommodatable] object?[] args);

        ValueTask InvokeVoidAsync(string identifier, [Accommodatable] object?[] args);
        ValueTask InvokeVoidAsync(string identifier, [Cancellable] CancellationToken cancellationToken, [Accommodatable] object?[] args);
        ValueTask InvokeVoidAsync(string identifier, [Cancellable] TimeSpan timeout, [Accommodatable] object?[] args);
    }
}
