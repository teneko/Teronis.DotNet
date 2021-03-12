using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    public readonly struct JSLocalObject : IAsyncDisposable, IJSLocalObject
    {
        public readonly IJSObjectReference JSObjectReference { get; }

        private readonly IJSFunctionalObject jsFunctionalObject;
        private readonly IJSLocalObjectActivator jsObjectActivator;

        public JSLocalObject(
            IJSFunctionalObject jsFunctionalObject,
            IJSObjectReference jsObjectReference,
            IJSLocalObjectActivator jsObjectActivator)
        {
            this.jsFunctionalObject = jsFunctionalObject ?? throw new ArgumentNullException(nameof(jsFunctionalObject));
            JSObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
            this.jsObjectActivator = jsObjectActivator ?? throw new ArgumentNullException(nameof(JSLocalObject.jsObjectActivator));
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

        public ValueTask DisposeAsync() =>
            JSObjectReference.DisposeAsync();
    }
}
