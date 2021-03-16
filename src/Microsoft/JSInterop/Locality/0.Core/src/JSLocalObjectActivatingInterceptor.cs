using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Locality
{
    internal class JSLocalObjectActivatingInterceptor : IJSFunctionalObjectInterceptor
    {
        static Type jsLocalObjectType = typeof(IJSLocalObject);

        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSLocalObjectActivatingInterceptor(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation)
        {
            if (invocation.Arguments.Length == 0 && jsLocalObjectType == invocation.GenericTaskArgumentType) {
                invocation.SetAlternativeResult((ValueTask<TValue>)(object)jsLocalObjectActivator.CreateInstanceAsync(invocation.Identifier));
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation) =>
            ValueTask.CompletedTask;
    }
}
