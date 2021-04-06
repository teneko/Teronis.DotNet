// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public class JSDynamicProxyReturningInterceptor : IJSInterceptor
    {
        private readonly IJSDynamicProxyActivator jsDynamicProxyActivator;

        public JSDynamicProxyReturningInterceptor(IJSDynamicProxyActivator jsDynamicProxyActivator) =>
            this.jsDynamicProxyActivator = jsDynamicProxyActivator ?? throw new ArgumentNullException(nameof(jsDynamicProxyActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            if (!invocation.InvocationAttributes.TryGetAttribute<ReturnDynamicProxyAttribute>(out var attribute)) {
                return;
            }

            var result = await invocation.GetNonDeterminedResult<IJSObjectReference>();
            var interfaceToBeProxied = attribute.InterfaceToBeProxied ?? invocation.TaskArgumentType;
            var determinedResult = (TValue)jsDynamicProxyActivator.CreateInstance(interfaceToBeProxied, result);
            invocation.SetAlternativeResult(new ValueTask<TValue>(determinedResult));
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
