using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    internal class JSLocalObjectActivatingInterceptor : IJSFunctionalObjectReferenceInterceptor
    {
        static Type jsLocalObjectType = typeof(IJSLocalObject);

        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSLocalObjectActivatingInterceptor(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public void TryInvokeAsync<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> invocation)
        {
            if (invocation.Arguments.Length == 0 && jsLocalObjectType == invocation.GenericTaskArgumentType) {
                invocation.SetInvocable((ValueTask<TValue>)(object)jsLocalObjectActivator.CreateLocalObjectAsync(invocation.Identifier));
            }
        }

        public void TryInvokeVoidAsync(ref JSFunctionalObjectReferenceInvocation invocation) { }
    }
}
