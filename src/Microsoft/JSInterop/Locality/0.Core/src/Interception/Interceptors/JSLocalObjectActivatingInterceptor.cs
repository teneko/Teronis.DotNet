using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Locality.Interception.Interceptors
{
    public class JSLocalObjectActivatingInterceptor : IJSFunctionalObjectInterceptor
    {
        private static Type jsLocalObjectType = typeof(IJSLocalObject);

        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSLocalObjectActivatingInterceptor(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public virtual ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation)
        {
            if (invocation.Arguments.Length == 0 && jsLocalObjectType == invocation.GenericTaskArgumentType) {
                invocation.SetAlternativeResult((ValueTask<TValue>)(object)jsLocalObjectActivator.CreateInstanceAsync(invocation.Identifier));
            }

            return ValueTask.CompletedTask;
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation) =>
            ValueTask.CompletedTask;
    }
}
