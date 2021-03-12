using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop
{
    public class JSInterceptableFunctionalObject : JSFunctionalObject
    {
        private readonly JSFunctionalObjectInterceptorWalker jsFunctionalObjectInterceptorWalker;

        public JSInterceptableFunctionalObject(JSFunctionalObjectInterceptorWalker jsFunctionalObjectInterceptorWalker) => 
            this.jsFunctionalObjectInterceptorWalker = jsFunctionalObjectInterceptorWalker;

        protected override void TryInvokeAsync<TValue>(ref JSFunctionalObjectInvocation<TValue> invocation) =>
            jsFunctionalObjectInterceptorWalker.WalkUntilCanInvoke(ref invocation);

        protected override void TryInvokeVoidAsync(ref JSFunctionalObjectInvocation invocation) =>
            jsFunctionalObjectInterceptorWalker.WalkUntilCanInvoke(ref invocation);
    }
}
