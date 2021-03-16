using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptableFunctionalObject : JSFunctionalObject
    {
        private readonly IJSFunctionalObjectInterceptor jsFunctionalObjectInterceptor;

        public JSInterceptableFunctionalObject(IJSFunctionalObjectInterceptor jsFunctionalObjectInterceptor) =>
            this.jsFunctionalObjectInterceptor = jsFunctionalObjectInterceptor;

        protected override ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation) =>
            jsFunctionalObjectInterceptor.InterceptInvokeAsync(invocation);

        protected override ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation) =>
            jsFunctionalObjectInterceptor.InterceptInvokeVoidAsync(invocation);
    }
}
