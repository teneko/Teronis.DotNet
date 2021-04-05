// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception.Interceptors.Builder;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObjectActivator : InterceptableFacadeActivatorBase, IJSLocalObjectActivator
    {
        private readonly IJSLocalObjectInterop jsLocalObjectInterop;

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop, JSInterceptorBuilder<JSLocalObjectInterceptorBuilderOptions>? interceptorBuilder)
            : base(interceptorBuilder) =>
            this.jsLocalObjectInterop = jsLocalObjectInterop ?? throw new System.ArgumentNullException(nameof(jsLocalObjectInterop));

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop)
            : this(jsLocalObjectInterop, interceptorBuilder: null) { }

        public virtual IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference, JSLocalObjectCreationOptions? creationOptions = null)
        {
            var jsInterceptor = BuildInterceptor();
            return new JSLocalObject(jsObjectReference, jsInterceptor);
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(string globalObjectName, JSLocalObjectCreationOptions? creationOptions = null)
        {
            var jsLocalObject = CreateInstance(await jsLocalObjectInterop.GetGlobalObjectReference(globalObjectName));
            return jsLocalObject;
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string localObjectName, JSLocalObjectCreationOptions? creationOptions = null)
        {
            var nestedObjectReference = await jsLocalObjectInterop.GetLocalObjectReference(jsObjectReference, localObjectName);
            var jsLocalObject = CreateInstance(nestedObjectReference, creationOptions);
            return jsLocalObject;
        }
    }
}
