// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public static class IJSInterceptorExtensions
    {
        public static async ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, customAttributes);
            await jsInterceptor.InterceptInvokeAsync(invocation);
            return await invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, params object?[] arguments) =>
            jsInterceptor.InvokeAsync<TValue>(jsObjectReference, identifier, arguments, customAttributes: null);

        public static async ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, customAttributes);
            await jsInterceptor.InterceptInvokeAsync(invocation);
            return await invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments) =>
            jsInterceptor.InvokeAsync<TValue>(jsObjectReference, identifier, cancellationToken, arguments, customAttributes: null);

        public static async ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation<TValue>(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, customAttributes);
            await jsInterceptor.InterceptInvokeAsync(invocation);
            return await invocation.GetDeterminedResult();
        }

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments) =>
            jsInterceptor.InvokeAsync<TValue>(jsObjectReference, identifier, timeout, arguments, customAttributes: null);

        public static async ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: null, arguments, customAttributes);
            await jsInterceptor.InterceptInvokeVoidAsync(invocation);
            await invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, params object?[] arguments) =>
            jsInterceptor.InvokeVoidAsync(jsObjectReference, identifier, arguments, customAttributes: null);

        public static async ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: cancellationToken, timeout: null, arguments, customAttributes);
            await jsInterceptor.InterceptInvokeVoidAsync(invocation);
            await invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, CancellationToken cancellationToken, params object?[] arguments) =>
            jsInterceptor.InvokeVoidAsync(jsObjectReference, identifier, cancellationToken, arguments, customAttributes: null);

        public static async ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, object?[] arguments, ICustomAttributes? customAttributes)
        {
            var invocation = new JSObjectInvocation(jsObjectReference, identifier, cancellationToken: null, timeout: timeout, arguments, customAttributes);
            await jsInterceptor.InterceptInvokeVoidAsync(invocation);
            await invocation.GetDeterminedResult();
        }

        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor, IJSObjectReference jsObjectReference, string identifier, TimeSpan timeout, params object?[] arguments) =>
            jsInterceptor.InvokeVoidAsync(jsObjectReference, identifier, timeout, arguments, customAttributes: null);
    }
}
