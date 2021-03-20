using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSCallingBackFunctionalObjectInterceptor : JSFunctionalObjectInterceptor
    {
        private readonly Func<IJSFunctionalObjectInvocation, ValueTask>? invokeVoidInvocationCallback;

        public JSCallingBackFunctionalObjectInterceptor(
            Func<IJSFunctionalObjectInvocation, ValueTask>? invokeVoidInvocationCallback) =>
            this.invokeVoidInvocationCallback = invokeVoidInvocationCallback;

        public override ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation) =>
            ValueTask.CompletedTask;

        public override ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation) =>
            invokeVoidInvocationCallback?.Invoke(invocation) ?? ValueTask.CompletedTask;
    }
}
