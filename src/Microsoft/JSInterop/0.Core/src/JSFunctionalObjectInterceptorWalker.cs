using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public sealed class JSFunctionalObjectInterceptorWalker : IJSFunctionalObjectInterceptor
    {
        private readonly IReadOnlyList<IJSFunctionalObjectInterceptor> interceptors;

        public JSFunctionalObjectInterceptorWalker(IReadOnlyList<IJSFunctionalObjectInterceptor> interceptors) =>
            this.interceptors = interceptors;

        public void WalkUntilCanInvoke<TValue>(ref JSFunctionalObjectInvocation<TValue> invocation)
        {
            foreach (var interception in interceptors) {
                interception.TryInvokeAsync(ref invocation);

                if (invocation.IsPromiseRedeemable) {
                    return;
                }
            }
        }

        public void WalkUntilCanInvoke(ref JSFunctionalObjectInvocation invocation)
        {
            foreach (var interception in interceptors) {
                interception.TryInvokeVoidAsync(ref invocation);


                if (invocation.CanInvoke) {
                    return;
                }
            }
        }

        void IJSFunctionalObjectInterceptor.TryInvokeAsync<TValue>(ref JSFunctionalObjectInvocation<TValue> invocation) =>
            WalkUntilCanInvoke(ref invocation);

        void IJSFunctionalObjectInterceptor.TryInvokeVoidAsync(ref JSFunctionalObjectInvocation invocation) =>
            WalkUntilCanInvoke(ref invocation);
    }
}
