// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSCallingBackObjectInterceptor : JSObjectInterceptor
    {
        private readonly Func<IJSObjectInvocation, ValueTask>? invokeVoidInvocationCallback;

        public JSCallingBackObjectInterceptor(
            Func<IJSObjectInvocation, ValueTask>? invokeVoidInvocationCallback) =>
            this.invokeVoidInvocationCallback = invokeVoidInvocationCallback;

        public override ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            ValueTask.CompletedTask;

        public override ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            invokeVoidInvocationCallback?.Invoke(invocation) ?? ValueTask.CompletedTask;
    }
}
