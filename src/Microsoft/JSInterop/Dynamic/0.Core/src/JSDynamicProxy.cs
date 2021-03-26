// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
