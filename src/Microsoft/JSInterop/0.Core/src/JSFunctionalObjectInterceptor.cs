using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public abstract class JSFunctionalObjectInterceptor : IJSFunctionalObjectInterceptor
    {
        public abstract void TryInvokeAsync<TValue>(ref JSFunctionalObjectInvocation<TValue> invocation);
        public abstract void TryInvokeVoidAsync(ref JSFunctionalObjectInvocation invocation);
    }
}
