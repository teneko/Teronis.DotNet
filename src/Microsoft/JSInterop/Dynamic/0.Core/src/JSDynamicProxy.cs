// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicProxy : JSObjectReferenceFacade, IJSObjectReferenceFacade
    {
        public JSDynamicProxy(IJSObjectReference jsObjectReference, IJSObjectInterceptor jsObjectInterceptor)
            : base(jsObjectReference, jsObjectInterceptor) { }

        public JSDynamicProxy(IJSObjectReference jsObjectReference)
            : base(jsObjectReference) { }
    }
}
