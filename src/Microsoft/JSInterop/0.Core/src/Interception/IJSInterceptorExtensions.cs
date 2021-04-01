// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class IJSInterceptorExtensions
    {
        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, customAttributes);
            jsInterceptor.InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, params object?[] arguments) =>
            InvokeAsync<TValue>(jsInterceptor, jsObjectReference, identifier, arguments, customAttributes: null);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, customAttributes);
            jsInterceptor.InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments) =>
            InvokeAsync<TValue>(jsInterceptor, jsObjectReference, identifier, cancellationToken, arguments, customAttributes: null);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, customAttributes);
            jsInterceptor.InterceptInvokeAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments) =>
            InvokeAsync<TValue>(jsInterceptor, jsObjectReference, identifier, timeout, arguments, customAttributes: null);

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, customAttributes);
            jsInterceptor.InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, params object?[] arguments) =>
            InvokeVoidAsync(jsInterceptor, jsObjectReference, identifier, arguments, customAttributes: null);

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, customAttributes);
            jsInterceptor.InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments) =>
            InvokeVoidAsync(jsInterceptor, jsObjectReference, identifier, cancellationToken, arguments, customAttributes: null);

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, customAttributes);
            jsInterceptor.InterceptInvokeVoidAsync(invocation);
            return invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments) =>
            InvokeVoidAsync(jsInterceptor, jsObjectReference, identifier, timeout, arguments, customAttributes: null);
    }
}
