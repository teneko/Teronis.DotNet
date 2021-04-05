// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    /// <summary>
    /// The interceptor mimics (obj_ref, name) => obj_ref[name] where name can be a dotted path too.
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

            if (invocation.Arguments.Length > 0) {
                throw new ArgumentException("When you mimic a getter you cannot specify arguments.");
            }

            var jsObjectReference = invocation.ObjectReference;
            var globalObjectNameOrPath = attribute.NameOrPath ?? invocation.Identifier;
            var jsLocalObject = await jsLocalObjectActivator.CreateInstanceAsync(jsObjectReference, globalObjectNameOrPath);
            invocation.SetAlternativeResult((TValue)jsLocalObject);
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
