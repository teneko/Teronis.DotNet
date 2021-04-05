// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interceptors
{
    public class JSDynamicProxyActivatingInterceptor : IJSInterceptor
    {
        private readonly IJSDynamicProxyActivator jsDynamicProxyActivator;

        public JSDynamicProxyActivatingInterceptor(IJSDynamicProxyActivator jsDynamicProxyActivator) =>
            this.jsDynamicProxyActivator = jsDynamicProxyActivator ?? throw new ArgumentNullException(nameof(jsDynamicProxyActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            if (invocation.InvocationAttributes.IsAttributeDefined(typeof(ReturnDynamicProxyAttribute))) {
                var result = await invocation.GetNonDeterminedResult<IJSObjectReference>();
                var determinedResult = (TValue)jsDynamicProxyActivator.CreateInstance(invocation.TaskArgumentType, result);
                invocation.SetAlternativeResult(new ValueTask<TValue>(determinedResult));
            }
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
