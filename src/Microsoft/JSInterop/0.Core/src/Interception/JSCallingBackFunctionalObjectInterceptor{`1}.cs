// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSCallingBackFunctionalObjectInterceptor<ValueTaskArgumentType> : JSCallingBackFunctionalObjectInterceptor
    {
        private readonly Func<IJSObjectInvocation<ValueTaskArgumentType>, ValueTask>? invokeInvocationCallback;

        public JSCallingBackFunctionalObjectInterceptor(
            Func<IJSObjectInvocation<ValueTaskArgumentType>, ValueTask>? invokeInvocationCallback,
            Func<IJSObjectInvocation, ValueTask>? invokeVoidInvocationCallback)
            : base(invokeVoidInvocationCallback) =>
            this.invokeInvocationCallback = invokeInvocationCallback;

        public JSCallingBackFunctionalObjectInterceptor(
            Func<IJSObjectInvocation<ValueTaskArgumentType>, ValueTask>? invokeInvocationCallback)
            : base(invokeVoidInvocationCallback: null) =>
            this.invokeInvocationCallback = invokeInvocationCallback;

        public override ValueTask InterceptInvokeAsync<RuntimeValueTaskArgumentType>(IJSObjectInvocation<RuntimeValueTaskArgumentType> invocation)
        {
            if (typeof(ValueTaskArgumentType) != typeof(RuntimeValueTaskArgumentType)) {
                throw new NotSupportedException($"The callback that has been passed at construction time can only intercept generic invocations whose value task argument type is {typeof(ValueTaskArgumentType)}.");
            }

            return invokeInvocationCallback?.Invoke((IJSObjectInvocation<ValueTaskArgumentType>)invocation) ?? ValueTask.CompletedTask;
        }
    }
}
