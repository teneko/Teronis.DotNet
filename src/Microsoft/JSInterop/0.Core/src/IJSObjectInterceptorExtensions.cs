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
    public static class IJSObjectInterceptorExtensions
    {
        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectInterceptor interceptor, IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            interceptor.InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectInterceptor interceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            interceptor.InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSObjectInterceptor interceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, EmptyCustomAttributeLookup.Instance);
            interceptor.InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSObjectInterceptor interceptor, IJSObjectReference jsObjectReference, string identifier, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            interceptor.InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSObjectInterceptor interceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, EmptyCustomAttributeLookup.Instance);
            interceptor.InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSObjectInterceptor interceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, EmptyCustomAttributeLookup.Instance);
            interceptor.InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }
    }
}
