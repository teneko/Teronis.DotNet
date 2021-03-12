using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public abstract class JSFunctionalObjectReferenceInterceptor : IJSFunctionalObjectReferenceInterceptor
    {
        public abstract void TryInvokeAsync<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> invocation);
        public abstract void TryInvokeVoidAsync(ref JSFunctionalObjectReferenceInvocation invocation);
    }
}
