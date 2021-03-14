using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectProxy : IJSDynamicObject
    {
        public IJSObjectReference JSObjectReference =>
            jsObjectReference;

        private readonly IJSObjectReference jsObjectReference;
        private readonly IJSFunctionalObject jsFunctionalObject;

        internal JSDynamicObjectProxy(IJSObjectReference jsObjectReference, IJSFunctionalObject jsFunctionalObject)
        {
            this.jsObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
            this.jsFunctionalObject = jsFunctionalObject;
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] arguments) =>
           jsFunctionalObject.InvokeAsync<TValue>(jsObjectReference, identifier, arguments);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            jsFunctionalObject.InvokeAsync<TValue>(jsObjectReference, identifier, cancellationToken, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, params object?[] args) =>
            jsFunctionalObject.InvokeAsync<TValue>(jsObjectReference, identifier, timeout, args);

        public ValueTask InvokeVoidAsync(string identifier, object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(jsObjectReference, identifier, args);

        public ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(jsObjectReference, identifier, cancellationToken, args);

        public ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(jsObjectReference, identifier, timeout, args);

        public ValueTask DisposeAsync() =>
            jsObjectReference.DisposeAsync();
    }
}
