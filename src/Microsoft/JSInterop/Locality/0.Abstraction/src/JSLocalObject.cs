using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObject : IAsyncDisposable, IJSLocalObject
    {
        public IJSObjectReference JSObjectReference { get; }

        private readonly IJSFunctionalObject jsFunctionalObject;

        public JSLocalObject(
            IJSFunctionalObject jsFunctionalObject,
            IJSObjectReference jsObjectReference)
        {
            this.jsFunctionalObject = jsFunctionalObject ?? throw new ArgumentNullException(nameof(jsFunctionalObject));
            JSObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] args) =>
            jsFunctionalObject.InvokeAsync<TValue>(JSObjectReference, identifier, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[] args) =>
            jsFunctionalObject.InvokeAsync<TValue>(JSObjectReference, identifier, cancellationToken, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, object?[] args) =>
            jsFunctionalObject.InvokeAsync<TValue>(JSObjectReference, identifier, timeout, args); 
        
        public ValueTask InvokeVoidAsync(string identifier, params object?[] args) =>
             jsFunctionalObject.InvokeVoidAsync(JSObjectReference, identifier, args);

        public ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(JSObjectReference, identifier, cancellationToken, args);

        public ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, params object?[] args) =>
            jsFunctionalObject.InvokeVoidAsync(JSObjectReference, identifier, timeout, args);

        protected virtual ValueTask DisposeAsyncCore() =>
            JSObjectReference.DisposeAsync();

        public ValueTask DisposeAsync() =>
            DisposeAsyncCore();
    }
}
