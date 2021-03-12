using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectReferenceInterceptor
    {
        void TryInvokeAsync<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> invocation);
        void TryInvokeVoidAsync(ref JSFunctionalObjectReferenceInvocation invocation);
    }
}
