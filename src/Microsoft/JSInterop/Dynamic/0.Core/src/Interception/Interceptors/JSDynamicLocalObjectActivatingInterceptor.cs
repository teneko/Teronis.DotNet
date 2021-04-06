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
    public class JSDynamicLocalObjectActivatingInterceptor : IJSInterceptor
    {
        private readonly IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator;

        public JSDynamicLocalObjectActivatingInterceptor(IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator) =>
            this.jsDynamicLocalObjectActivator = jsDynamicLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsDynamicLocalObjectActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            if (!invocation.InvocationAttributes.TryGetAttribute<ActivateDynamicLocalObjectAttribute>(out var attribute)) {
                return;
            }

            invocation.EnsureMimickingGetter();
            var jsObjectReference = invocation.ObjectReference;
            var globalObjectNameOrPath = attribute.NameOrPath ?? invocation.Identifier;
            var interfaceToBeProxied = attribute.InterfaceToBeProxied ?? invocation.TaskArgumentType;
            var jsLocalObject = await jsDynamicLocalObjectActivator.CreateInstanceAsync(interfaceToBeProxied, jsObjectReference, globalObjectNameOrPath);
            invocation.SetAlternativeResult((TValue)jsLocalObject);
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
