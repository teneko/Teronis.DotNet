// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public class JSDynamicLocalObjectReturningInterceptor : IJSInterceptor
    {
        private readonly IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator;

        public JSDynamicLocalObjectReturningInterceptor(IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator) =>
            this.jsDynamicLocalObjectActivator = jsDynamicLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsDynamicLocalObjectActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            if (!invocation.InvocationAttributes.TryGetAttribute<ReturnDynamicLocalObjectAttribute>(out var attribute)) {
                return;
            }

            var jsObjectReference = await invocation.GetNonDeterminedResult<IJSObjectReference>();
            var globalObjectNameOrPath = attribute.NameOrPath ?? invocation.Identifier;
            var interfaceToBeProxied = attribute.InterfaceToBeProxied ?? invocation.TaskArgumentType;
            var jsLocalObject = await jsDynamicLocalObjectActivator.CreateInstanceAsync(interfaceToBeProxied, jsObjectReference, globalObjectNameOrPath);
            invocation.SetAlternativeResult((TValue)jsLocalObject);
        }

        public ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
