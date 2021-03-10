using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public static class IJSObjectReferenceFacadeExtensions
    {
        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeAsync<TValue>(jsObjectReferenceFacade.JSObjectReference, identifier, args);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, CancellationToken cancellationToken, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeAsync<TValue>(jsObjectReferenceFacade.JSObjectReference, identifier, cancellationToken, args);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, TimeSpan timeout, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeAsync<TValue>(jsObjectReferenceFacade.JSObjectReference, identifier, timeout, args);

        public static ValueTask InvokeVoidAsync(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeVoidAsync(jsObjectReferenceFacade.JSObjectReference, identifier, args);

        public static ValueTask InvokeVoidAsync(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, CancellationToken cancellationToken, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeVoidAsync(jsObjectReferenceFacade.JSObjectReference, identifier, cancellationToken, args);

        public static ValueTask InvokeVoidAsync(this IJSObjectReferenceFacade jsObjectReferenceFacade, string identifier, TimeSpan timeout, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeVoidAsync(jsObjectReferenceFacade.JSObjectReference, identifier, timeout, args);
    }
}
