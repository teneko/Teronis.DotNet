using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectReference : IJSFunctionalObjectReference, IJSFunctionalObjectReferenceInterceptor
    {
        public static JSFunctionalObjectReference Default = new JSFunctionalObjectReference();

        protected virtual void TryInvokeAsync<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> jsObjectReference) { }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectReferenceInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeSpan: null, arguments);
            TryInvokeAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectReferenceInvocation<TValue>(jsObjectReference, identifier, cancellationToken: cancellationToken, timeSpan: null, arguments);
            TryInvokeAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectReferenceInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeSpan: timeout, arguments);
            TryInvokeAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        protected virtual void TryInvokeVoidAsync(ref JSFunctionalObjectReferenceInvocation invocation) { }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectReferenceInvocation(jsObjectReference, identifier, cancellationToken: null, timeSpan: null, arguments);
            TryInvokeVoidAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectReferenceInvocation(jsObjectReference, identifier, cancellationToken: cancellationToken, timeSpan: null, arguments);
            TryInvokeVoidAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectReferenceInvocation(jsObjectReference, identifier, cancellationToken: null, timeSpan: timeout, arguments);
            TryInvokeVoidAsync(ref invocation);
            return invocation.InvokeAsync();
        }

        #region IJSFunctionalObjectReferenceInterception

        void IJSFunctionalObjectReferenceInterceptor.TryInvokeAsync<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> invocation) =>
            TryInvokeAsync(ref invocation);

        void IJSFunctionalObjectReferenceInterceptor.TryInvokeVoidAsync(ref JSFunctionalObjectReferenceInvocation invocation) =>
            TryInvokeVoidAsync(ref invocation);

        #endregion
    }
}
