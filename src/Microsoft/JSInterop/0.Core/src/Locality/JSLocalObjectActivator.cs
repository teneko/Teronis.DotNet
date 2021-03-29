// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObjectActivator : IInstanceActivatorBase<IJSLocalObject>, IJSLocalObjectActivator
    {
        private readonly IJSLocalObjectInterop jsLocalObjectInterop;
        private readonly GetOrBuildJSInterceptableFunctionalObjectDelegate? getOrBuildJSFunctionalObjectDelegate;

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop, JSLocalObjectActivatorOptions? options)
        {
            this.jsLocalObjectInterop = jsLocalObjectInterop ?? throw new System.ArgumentNullException(nameof(jsLocalObjectInterop));
            getOrBuildJSFunctionalObjectDelegate = options?.GetOrBuildJSInterceptableFunctionalObject;
        }

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop)
            : this(jsLocalObjectInterop, options: null) { }

        public virtual IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference)
        {
            var jsFunctionalObject = getOrBuildJSFunctionalObjectDelegate?.Invoke(configureInterceptorWalkerBuilder: null) ?? JSFunctionalObject.Default;
            return new JSLocalObject(jsFunctionalObject, jsObjectReference);
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName)
        {
            var jsLocalObject = CreateInstance(await jsLocalObjectInterop.GetGlobalObjectReference(objectName));
            DispatchInstanceActicated(jsLocalObject);
            return jsLocalObject;
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string objectName)
        {
            var nestedObjectReference = await jsLocalObjectInterop.GetLocalObjectReference(jsObjectReference, objectName);
            var jsLocalObject = CreateInstance(nestedObjectReference);
            DispatchInstanceActicated(jsLocalObject);
            return jsLocalObject;
        }
    }
}
