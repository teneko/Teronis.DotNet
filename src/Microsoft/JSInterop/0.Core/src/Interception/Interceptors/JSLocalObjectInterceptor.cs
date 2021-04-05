// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    /// <summary>
    /// The interceptor mimics (obj_ref, name) => obj_ref[name] where name can be a path too.
    /// </summary>
    public class JSLocalObjectInterceptor : IJSInterceptor
    {
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSLocalObjectInterceptor(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            if (!invocation.InvocationAttributes.TryGetAttribute<ReturnLocalObjectAttribute>(out var attribute)) {
                return;
            }

            var jsObjectReference = await invocation.GetNonDeterminedResult<IJSObjectReference>();
            var globalObjectNameOrPath = attribute.NameOrPath ?? invocation.Identifier;
            var jsLocalObject = await jsLocalObjectActivator.CreateInstanceAsync(jsObjectReference, globalObjectNameOrPath);
            invocation.SetAlternativeResult((TValue)jsLocalObject);
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
