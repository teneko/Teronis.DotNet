// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObjectActivator : InstanceActivatorBase<IJSLocalObject>, IJSLocalObjectActivator
    {
        private readonly IJSLocalObjectInterop jsLocalObjectInterop;
        private readonly GetOrBuildInterceptorDelegate? getOrBuildInterceptorDelegate;

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop, JSLocalObjectActivatorOptions? options)
        {
            this.jsLocalObjectInterop = jsLocalObjectInterop ?? throw new System.ArgumentNullException(nameof(jsLocalObjectInterop));
            getOrBuildInterceptorDelegate = options?.GetOrBuildInterceptorMethod;
        }

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop)
            : this(jsLocalObjectInterop, options: null) { }

        public virtual IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference)
        {
            var jsObjectInterceptor = getOrBuildInterceptorDelegate?.Invoke(configureBuilder: null) ?? JSObjectInterceptor.Default;
            return new JSLocalObject(jsObjectInterceptor, jsObjectReference);
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
