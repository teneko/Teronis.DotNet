using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public class JSObjectReferenceFacade : IJSObjectReferenceFacade
    {
        public IJSObjectReference JSObjectReference { get; }

        protected virtual IJSFunctionalObject JSFunctionalObject { get; }

        public JSObjectReferenceFacade(IJSFunctionalObject? jsFunctionalObject, IJSObjectReference jsObjectReference)
        {
            JSFunctionalObject = jsFunctionalObject ?? JSInterop.JSFunctionalObject.Default;
            JSObjectReference = jsObjectReference ?? throw new ArgumentNullException(nameof(jsObjectReference));
        }

        public JSObjectReferenceFacade(IJSObjectReference jsObjectReference)
            : this(jsFunctionalObject: null, jsObjectReference) { }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[] args) =>
            JSFunctionalObject.InvokeAsync<TValue>(JSObjectReference, identifier, args);

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            JSFunctionalObject.InvokeAsync<TValue>(JSObjectReference, identifier, cancellationToken, args);

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, TimeSpan timeout, params object?[] args) =>
            JSFunctionalObject.InvokeAsync<TValue>(JSObjectReference, identifier, timeout, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, params object?[] args) =>
            JSFunctionalObject.InvokeVoidAsync(JSObjectReference, identifier, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args) =>
            JSFunctionalObject.InvokeVoidAsync(JSObjectReference, identifier, cancellationToken, args);

        public virtual ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, params object?[] args) =>
            JSFunctionalObject.InvokeVoidAsync(JSObjectReference, identifier, timeout, args);
    }
}
