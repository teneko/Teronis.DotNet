// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObject : IAsyncDisposable, IJSLocalObject
    {
        public IJSObjectReference JSObjectReference { get; }

        private readonly IJSFunctionalObject jsObjectInterceptor;

        public JSLocalObject(
            IJSFunctionalObject jsObjectInterceptor,
            IJSObjectReference jsObjectReference)
        {
            this.jsObjectInterceptor = jsObjectInterceptor ?? throw new ArgumentNullException(nameof(jsObjectInterceptor));
            JSObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] args) =>
            jsObjectInterceptor.InvokeAsync<TValue>(JSObjectReference, identifier, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[] args) =>
            jsObjectInterceptor.InvokeAsync<TValue>(JSObjectReference, identifier, cancellationToken, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, object?[] args) =>
            jsObjectInterceptor.InvokeAsync<TValue>(JSObjectReference, identifier, timeout, args); 
        
        public ValueTask InvokeVoidAsync(string identifier, params object?[] args) =>
             jsObjectInterceptor.InvokeVoidAsync(JSObjectReference, identifier, args);

        public ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            jsObjectInterceptor.InvokeVoidAsync(JSObjectReference, identifier, cancellationToken, args);

        public ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, params object?[] args) =>
            jsObjectInterceptor.InvokeVoidAsync(JSObjectReference, identifier, timeout, args);

        protected virtual ValueTask DisposeAsyncCore() =>
            JSObjectReference.DisposeAsync();

        public ValueTask DisposeAsync() =>
            DisposeAsyncCore();
    }
}
