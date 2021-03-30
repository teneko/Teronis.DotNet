// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSCallingBackObjectInterceptor<TaskArgumentType> : JSCallingBackObjectInterceptor
    {
        private readonly Func<IJSObjectInvocation<TaskArgumentType>, ValueTask>? invokeInvocationCallback;

        public JSCallingBackObjectInterceptor(
            Func<IJSObjectInvocation<TaskArgumentType>, ValueTask>? invokeInvocationCallback,
            Func<IJSObjectInvocation, ValueTask>? invokeVoidInvocationCallback)
            : base(invokeVoidInvocationCallback) =>
            this.invokeInvocationCallback = invokeInvocationCallback;

        public JSCallingBackObjectInterceptor(
            Func<IJSObjectInvocation<TaskArgumentType>, ValueTask>? invokeInvocationCallback)
            : base(invokeVoidInvocationCallback: null) =>
            this.invokeInvocationCallback = invokeInvocationCallback;

        public override ValueTask InterceptInvokeAsync<RuntimeValueTaskArgumentType>(IJSObjectInvocation<RuntimeValueTaskArgumentType> invocation)
        {
            if (typeof(TaskArgumentType) != typeof(RuntimeValueTaskArgumentType)) {
                throw new NotSupportedException($"The callback that has been passed at construction time can only intercept generic invocations whose value task argument type is {typeof(TaskArgumentType)}.");
            }

            return invokeInvocationCallback?.Invoke((IJSObjectInvocation<TaskArgumentType>)invocation) ?? ValueTask.CompletedTask;
        }
    }
}
