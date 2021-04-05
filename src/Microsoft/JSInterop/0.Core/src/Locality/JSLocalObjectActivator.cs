// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObjectActivator : InterceptableFacadeActivatorBase, IJSLocalObjectActivator
    {
        private readonly IJSLocalObjectInterop jsLocalObjectInterop;

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop, JSInterceptorBuilder<JSLocalObjectInterceptorServicesOptions>? interceptorBuilder)
            : base(interceptorBuilder) =>
            this.jsLocalObjectInterop = jsLocalObjectInterop ?? throw new System.ArgumentNullException(nameof(jsLocalObjectInterop));

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop)
            : this(jsLocalObjectInterop, interceptorBuilder: null) { }

        public virtual IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference, JSLocalObjectCreationOptions? creationOptions = null)
        {
            var jsInterceptor = BuildInterceptor();
            return new JSLocalObject(jsObjectReference, jsInterceptor);
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(string globalObjectNameOrPath, JSLocalObjectCreationOptions? creationOptions = null)
        {
            var jsLocalObject = CreateInstance(await jsLocalObjectInterop.GetGlobalObjectReference(globalObjectNameOrPath));
            return jsLocalObject;
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string localObjectNameOrPath, JSLocalObjectCreationOptions? creationOptions = null)
        {
            var nestedObjectReference = await jsLocalObjectInterop.GetLocalObjectReference(jsObjectReference, localObjectNameOrPath);
            var jsLocalObject = CreateInstance(nestedObjectReference, creationOptions);
            return jsLocalObject;
        }
    }
}
