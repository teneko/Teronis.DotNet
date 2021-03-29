// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObject: IJSObjectInterceptor
    {
        ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, object?[] arguments);
        ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] args);
        ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] args);

        ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, object?[] args);
        ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] args);
        ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] args);
    }
}
