// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor
{
    public class JSCallingBackInterceptor : JSInterceptor
    {
        private readonly Func<IJSObjectInvocation, InterceptionContext, ValueTask>? invokeVoidInvocationWithContextCallback;

        public JSCallingBackInterceptor(
            Func<IJSObjectInvocation, InterceptionContext, ValueTask>? invokeVoidInvocationWithContextCallback) =>
            this.invokeVoidInvocationWithContextCallback = invokeVoidInvocationWithContextCallback;

        public override ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;

        public override ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context)
        {
            return invokeVoidInvocationWithContextCallback?.Invoke(invocation, context)
                ?? ValueTask.CompletedTask;
        }
    }
}
