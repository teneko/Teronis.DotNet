// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObject : IJSFunctionalObject, IJSObjectInterceptor
    {
        public static JSFunctionalObject Default;

        static JSFunctionalObject() =>
            Default = new JSFunctionalObject();

        protected virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> jsObjectReference) =>
            ValueTask.CompletedTask;

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, EmptyCustomAttributeLookup.Instance);
            InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        protected virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            ValueTask.CompletedTask;

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public virtual ValueTask InvokeVoidAsync(IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, EmptyCustomAttributeLookup.Instance);
            InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        #region IJSFunctionalObjectInterception

        ValueTask IJSObjectInterceptor.InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            InterceptInvokeAsync(invocation);

        ValueTask IJSObjectInterceptor.InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            InterceptInvokeVoidAsync(invocation);

        #endregion
    }
}
