using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public class JSInterceptableFunctionalObjectReference : JSFunctionalObjectReference
    {
        private readonly JSFunctionalObjectReferenceInterceptorWalker jsFunctionalObjectReferenceInterceptorWalker;

        public JSInterceptableFunctionalObjectReference(JSFunctionalObjectReferenceInterceptorWalker jsFunctionalObjectReferenceInterceptorWalker) => 
            this.jsFunctionalObjectReferenceInterceptorWalker = jsFunctionalObjectReferenceInterceptorWalker;

        protected override void TryInvokeAsync<TValue>(ref JSFunctionalObjectReferenceInvocation<TValue> invocation) =>
            jsFunctionalObjectReferenceInterceptorWalker.WalkUntilCanInvoke(ref invocation);

        protected override void TryInvokeVoidAsync(ref JSFunctionalObjectReferenceInvocation invocation) =>
            jsFunctionalObjectReferenceInterceptorWalker.WalkUntilCanInvoke(ref invocation);
    }
}
