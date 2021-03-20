using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSCallingBackFunctionalObjectInterceptor<ValueTaskArgumentType> : JSCallingBackFunctionalObjectInterceptor
    {
        private readonly Func<IJSFunctionalObjectInvocation<ValueTaskArgumentType>, ValueTask>? invokeInvocationCallback;

        public JSCallingBackFunctionalObjectInterceptor(
            Func<IJSFunctionalObjectInvocation<ValueTaskArgumentType>, ValueTask>? invokeInvocationCallback,
            Func<IJSFunctionalObjectInvocation, ValueTask>? invokeVoidInvocationCallback)
            : base(invokeVoidInvocationCallback) =>
            this.invokeInvocationCallback = invokeInvocationCallback;

        public JSCallingBackFunctionalObjectInterceptor(
            Func<IJSFunctionalObjectInvocation<ValueTaskArgumentType>, ValueTask>? invokeInvocationCallback)
            : base(invokeVoidInvocationCallback: null) =>
            this.invokeInvocationCallback = invokeInvocationCallback;

        public override ValueTask InterceptInvokeAsync<RuntimeValueTaskArgumentType>(IJSFunctionalObjectInvocation<RuntimeValueTaskArgumentType> invocation)
        {
            if (typeof(ValueTaskArgumentType) != typeof(RuntimeValueTaskArgumentType)) {
                throw new NotSupportedException($"The callback that has been passed at construction time can only intercept generic invocations whose value task argument type is {typeof(ValueTaskArgumentType)}.");
            }

            return invokeInvocationCallback?.Invoke((IJSFunctionalObjectInvocation<ValueTaskArgumentType>)invocation) ?? ValueTask.CompletedTask;
        }
    }
}
