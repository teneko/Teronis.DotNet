using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObject : IJSFunctionalObject, IJSFunctionalObjectInterceptor
    {
        public static JSFunctionalObject Default;

        static JSFunctionalObject() =>
            Default = new JSFunctionalObject();

        internal static JSFunctionalObject GetDefault() =>
            Default;

        protected virtual ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> jsObjectReference) =>
            ValueTask.CompletedTask;

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments);
            InterceptInvokeAsync(invocation);
            return invocation.GetResult();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments);
            InterceptInvokeAsync(invocation);
            return invocation.GetResult();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments);
            InterceptInvokeAsync(invocation);
            return invocation.GetResult();
        }

        protected virtual ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation) =>
            ValueTask.CompletedTask;

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments);
            InterceptInvokeVoidAsync(invocation);
            return invocation.GetResult();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments);
            InterceptInvokeVoidAsync(invocation);
            return invocation.GetResult();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSFunctionalObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments);
            InterceptInvokeVoidAsync(invocation);
            return invocation.GetResult();
        }

        #region IJSFunctionalObjectInterception

        ValueTask IJSFunctionalObjectInterceptor.InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation) =>
            InterceptInvokeAsync(invocation);

        ValueTask IJSFunctionalObjectInterceptor.InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation) =>
            InterceptInvokeVoidAsync(invocation);

        #endregion
    }
}
