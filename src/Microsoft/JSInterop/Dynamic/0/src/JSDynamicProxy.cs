using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicProxy : JSObjectReferenceFacade, IJSObjectReferenceFacade
    {
        public JSDynamicProxy(IJSObjectReference jsObjectReference, IJSFunctionalObject jsFunctionalObject)
            : base(jsObjectReference, jsFunctionalObject) { }

        public JSDynamicProxy(IJSObjectReference jsObjectReference)
            : base(jsObjectReference) { }
    }
}
