using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public abstract class JSFunctionalObjectInterceptor : IJSFunctionalObjectInterceptor
    {
        public abstract ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation);
        public abstract ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation);
    }
}
