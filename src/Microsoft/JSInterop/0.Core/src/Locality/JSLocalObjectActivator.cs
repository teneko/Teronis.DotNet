// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObjectActivator : IJSLocalObjectActivator
    {
        private readonly IJSLocalObjectInterop jsLocalObjectInterop;
        private readonly BuildInterceptorDelegate? buildInterceptor;

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop, IOptions<JSLocalObjectInterceptorBuilderOptions>? options)
        {
            this.jsLocalObjectInterop = jsLocalObjectInterop ?? throw new System.ArgumentNullException(nameof(jsLocalObjectInterop));
            buildInterceptor = options is null ? null : (BuildInterceptorDelegate?)options.Value.BuildInterceptor;
        }

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop)
            : this(jsLocalObjectInterop, options: null) { }

        public virtual IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference)
        {
            var jsObjectInterceptor = buildInterceptor
                ?.Invoke(
                    configureBuilder: null)
                ?? JSObjectInterceptor.Default;

            return new JSLocalObject(jsObjectReference, jsObjectInterceptor);
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName)
        {
            var jsLocalObject = CreateInstance(await jsLocalObjectInterop.GetGlobalObjectReference(objectName));
            return jsLocalObject;
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string objectName)
        {
            var nestedObjectReference = await jsLocalObjectInterop.GetLocalObjectReference(jsObjectReference, objectName);
            var jsLocalObject = CreateInstance(nestedObjectReference);
            return jsLocalObject;
        }
    }
}
