// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSCallingBackInterceptor<TaskArgumentType> : JSCallingBackInterceptor
    {
        private readonly Func<IJSObjectInvocation<TaskArgumentType>, InterceptionContext, ValueTask>? invokeInvocationWithContextCallback;

        public JSCallingBackInterceptor(
            Func<IJSObjectInvocation<TaskArgumentType>, InterceptionContext, ValueTask>? invokeInvocationWithContextCallback,
            Func<IJSObjectInvocation, InterceptionContext, ValueTask>? invokeVoidInvocationWithContextCallback)
            : base(invokeVoidInvocationWithContextCallback) =>
            this.invokeInvocationWithContextCallback = invokeInvocationWithContextCallback;

        public JSCallingBackInterceptor(
            Func<IJSObjectInvocation<TaskArgumentType>, InterceptionContext, ValueTask>? invokeInvocationWithContextCallback)
            : base(invokeVoidInvocationWithContextCallback: null) =>
            this.invokeInvocationWithContextCallback = invokeInvocationWithContextCallback;

        public override ValueTask InterceptInvokeAsync<RuntimeValueTaskArgumentType>(IJSObjectInvocation<RuntimeValueTaskArgumentType> invocation, InterceptionContext context)
        {
            if (typeof(TaskArgumentType) != typeof(RuntimeValueTaskArgumentType)) {
                throw new NotSupportedException($"The callback that has been passed at construction time can only intercept generic invocations whose value task argument type is {typeof(TaskArgumentType)}.");
            }

            return invokeInvocationWithContextCallback?.Invoke((IJSObjectInvocation<TaskArgumentType>)invocation, context) ?? ValueTask.CompletedTask;
        }
    }
}
