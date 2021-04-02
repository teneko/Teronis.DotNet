// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception.Interceptor;

namespace Teronis.Microsoft.JSInterop
{
    public class JSObjectReferenceFacade : IJSObjectReferenceFacade
    {
        public IJSObjectReference ObjectReference { get; }

        protected virtual IJSInterceptor Interceptor { get; }

        public JSObjectReferenceFacade(IJSObjectReference jsObjectReference, IJSInterceptor? jsInterceptor)
        {
            ObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
            Interceptor = jsInterceptor ?? JSInterceptor.Default;
        }

        public JSObjectReferenceFacade(IJSObjectReference jsObjectReference)
            : this(jsObjectReference, jsInterceptor: null) { }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[] args) =>
            Interceptor.InvokeAsync<TValue>(ObjectReference, identifier, args);

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            Interceptor.InvokeAsync<TValue>(ObjectReference, identifier, cancellationToken, args);

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, params object?[] args) =>
            Interceptor.InvokeAsync<TValue>(ObjectReference, identifier, timeout, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, params object?[] args) =>
            Interceptor.InvokeVoidAsync(ObjectReference, identifier, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            Interceptor.InvokeVoidAsync(ObjectReference, identifier, cancellationToken, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, params object?[] args) =>
            Interceptor.InvokeVoidAsync(ObjectReference, identifier, timeout, args);

        protected virtual ValueTask DisposeAsyncCore() =>
            ObjectReference.DisposeAsync();

        public ValueTask DisposeAsync() =>
            DisposeAsyncCore();

        IJSInterceptor IJSObjectReferenceFacade.Interceptor =>
            Interceptor;
    }
}
