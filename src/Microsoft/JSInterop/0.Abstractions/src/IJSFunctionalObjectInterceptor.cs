using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectInterceptor
    {
        void TryInvokeAsync<TValue>(ref JSFunctionalObjectInvocation<TValue> invocation);
        void TryInvokeVoidAsync(ref JSFunctionalObjectInvocation invocation);
    }
}
