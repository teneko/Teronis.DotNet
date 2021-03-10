using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public static class IJSLocalObjectExtensions
    {
        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSLocalObject jsObjectReference, string identifier, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeAsync<TValue>(jsObjectReference.JSObjectReference, identifier, args);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSLocalObject jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeAsync<TValue>(jsObjectReference.JSObjectReference, identifier, cancellationToken, args);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSLocalObject jsObjectReference, string identifier, TimeSpan timeout, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeAsync<TValue>(jsObjectReference.JSObjectReference, identifier, timeout, args);

        public static ValueTask InvokeVoidAsync(this IJSLocalObject jsObjectReference, string identifier, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeVoidAsync(jsObjectReference.JSObjectReference, identifier, args);

        public static ValueTask InvokeVoidAsync(this IJSLocalObject jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeVoidAsync(jsObjectReference.JSObjectReference, identifier, cancellationToken, args);

        public static ValueTask InvokeVoidAsync(this IJSLocalObject jsObjectReference, string identifier, TimeSpan timeout, params object?[] args) =>
            JSObjectReferenceExtensions.InvokeVoidAsync(jsObjectReference.JSObjectReference, identifier, timeout, args);
    }
}
