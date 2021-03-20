using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInterceptor
    {
        ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation);
        ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation);
    }
}
