// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSObjectReferenceFacade : IAsyncDisposable
    {
        IJSObjectReference Reference { get; }
        IJSInterceptor Interceptor { get; }

        ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] arguments);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[] arguments);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, object?[] arguments);

        ValueTask InvokeVoidAsync(string identifier, object?[] arguments);
        ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, object?[] arguments);
        ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, object?[] arguments);
    }
}
