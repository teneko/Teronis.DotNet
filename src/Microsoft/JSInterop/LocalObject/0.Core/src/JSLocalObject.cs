using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    public readonly struct JSLocalObject : IAsyncDisposable, IJSLocalObject
    {
        public readonly IJSObjectReference JSObjectReference { get; }

        private readonly IJSFunctionalObjectReference jsFunctionalObjectReference;
        private readonly IJSLocalObjectActivator jsObjectActivator;

        public JSLocalObject(
            IJSFunctionalObjectReference jsFunctionalObjectReference,
            IJSObjectReference jsObjectReference,
            IJSLocalObjectActivator jsObjectActivator)
        {
            this.jsFunctionalObjectReference = jsFunctionalObjectReference ?? throw new ArgumentNullException(nameof(jsFunctionalObjectReference));
            JSObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
            this.jsObjectActivator = jsObjectActivator ?? throw new ArgumentNullException(nameof(JSLocalObject.jsObjectActivator));
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[] args) =>
            jsFunctionalObjectReference.InvokeAsync<TValue>(JSObjectReference, identifier, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[] args) =>
            jsFunctionalObjectReference.InvokeAsync<TValue>(JSObjectReference, identifier, cancellationToken, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, object?[] args) =>
            jsFunctionalObjectReference.InvokeAsync<TValue>(JSObjectReference, identifier, timeout, args); 
        
        public ValueTask InvokeVoidAsync(string identifier, params object?[] args) =>
             jsFunctionalObjectReference.InvokeVoidAsync(JSObjectReference, identifier, args);

        public ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            jsFunctionalObjectReference.InvokeVoidAsync(JSObjectReference, identifier, cancellationToken, args);

        public ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, params object?[] args) =>
            jsFunctionalObjectReference.InvokeVoidAsync(JSObjectReference, identifier, timeout, args);

        public ValueTask DisposeAsync() =>
            JSObjectReference.DisposeAsync();
    }
}
