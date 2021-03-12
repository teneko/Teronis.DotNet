using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public sealed class JSFunctionalObjectReferenceInterceptorWalker : IJSFunctionalObjectReferenceInterceptor
    {
        private readonly IReadOnlyList<IJSFunctionalObjectReferenceInterceptor> interceptors;

        public JSFunctionalObjectReferenceInterceptorWalker(IReadOnlyList<IJSFunctionalObjectReferenceInterceptor> interceptors) =>
            this.interceptors = interceptors;

        public void WalkUntilCanInvoke<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> invocation)
        {
            foreach (var interception in interceptors) {
                interception.TryInvokeAsync(ref invocation);

                if (invocation.CanInvoke) {
                    return;
                }
            }
        }

        public void WalkUntilCanInvoke(ref JSFunctionalObjectReferenceInvocation invocation)
        {
            foreach (var interception in interceptors) {
                interception.TryInvokeVoidAsync(ref invocation);


                if (invocation.CanInvoke) {
                    return;
                }
            }
        }

        void IJSFunctionalObjectReferenceInterceptor.TryInvokeAsync<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> invocation) =>
            WalkUntilCanInvoke(ref invocation);

        void IJSFunctionalObjectReferenceInterceptor.TryInvokeVoidAsync(ref JSFunctionalObjectReferenceInvocation invocation) =>
            WalkUntilCanInvoke(ref invocation);
    }
}
