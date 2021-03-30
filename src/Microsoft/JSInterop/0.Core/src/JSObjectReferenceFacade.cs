// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public class JSObjectReferenceFacade : IJSObjectReferenceFacade
    {
        public IJSObjectReference JSObjectReference { get; }

        protected virtual IJSObjectInterceptor Interceptor { get; }

        public JSObjectReferenceFacade(IJSObjectReference jsObjectReference, IJSObjectInterceptor? jsObjectInterceptor)
        {
            JSObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
            Interceptor = jsObjectInterceptor ?? JSObjectInterceptor.Default;
        }

        public JSObjectReferenceFacade(IJSObjectReference jsObjectReference)
            : this(jsObjectReference, jsObjectInterceptor: null) { }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[] args) =>
            Interceptor.InvokeAsync<TValue>(JSObjectReference, identifier, args);

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            Interceptor.InvokeAsync<TValue>(JSObjectReference, identifier, cancellationToken, args);

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, params object?[] args) =>
            Interceptor.InvokeAsync<TValue>(JSObjectReference, identifier, timeout, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, params object?[] args) =>
            Interceptor.InvokeVoidAsync(JSObjectReference, identifier, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            Interceptor.InvokeVoidAsync(JSObjectReference, identifier, cancellationToken, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, params object?[] args) =>
            Interceptor.InvokeVoidAsync(JSObjectReference, identifier, timeout, args);

        protected virtual ValueTask DisposeAsyncCore() =>
            JSObjectReference.DisposeAsync();

        public ValueTask DisposeAsync() =>
            DisposeAsyncCore();
    }
}
