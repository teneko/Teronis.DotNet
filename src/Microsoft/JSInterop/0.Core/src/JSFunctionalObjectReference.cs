using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObject : IJSFunctionalObject, IJSFunctionalObjectInterceptor
    {
        public static JSFunctionalObject Default = new JSFunctionalObject();

        public static JSFunctionalObject GetDefault() => Default;

        protected virtual void TryInvokeAsync<TValue>(ref JSFunctionalObjectInvocation<TValue> jsObjectReference) { }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments);
            TryInvokeAsync(ref invocation);
            return invocation.GetPromise();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments);
            TryInvokeAsync(ref invocation);
            return invocation.GetPromise();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments);
            TryInvokeAsync(ref invocation);
            return invocation.GetPromise();
        }

        protected virtual void TryInvokeVoidAsync(ref JSFunctionalObjectInvocation invocation) { }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeSpan: null, arguments);
            TryInvokeVoidAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation(jsObjectReference, identifier, cancellationToken: cancellationToken, timeSpan: null, arguments);
            TryInvokeVoidAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeSpan: timeout, arguments);
            TryInvokeVoidAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        #region IJSFunctionalObjectInterception

        void IJSFunctionalObjectInterceptor.TryInvokeAsync<TValue>(ref JSFunctionalObjectInvocation<TValue> invocation) =>
            TryInvokeAsync(ref invocation);

        void IJSFunctionalObjectInterceptor.TryInvokeVoidAsync(ref JSFunctionalObjectInvocation invocation) =>
            TryInvokeVoidAsync(ref invocation);

        #endregion
    }
}
