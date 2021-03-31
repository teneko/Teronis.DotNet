// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObject : JSObjectReferenceFacade, IJSLocalObject
    {
        public JSLocalObject(IJSObjectReference jsObjectReference, IJSObjectInterceptor? jsObjectInterceptor)
            : base(jsObjectReference, jsObjectInterceptor) { }
    }
}
