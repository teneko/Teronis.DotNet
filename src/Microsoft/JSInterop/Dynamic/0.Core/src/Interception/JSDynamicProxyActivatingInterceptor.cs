// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSDynamicProxyActivatingInterceptor : IJSInterceptor
    {
        private readonly IJSDynamicProxyActivator jsDynamicProxyActivator;

        public JSDynamicProxyActivatingInterceptor(IJSDynamicProxyActivator jsDynamicProxyActivator) =>
            this.jsDynamicProxyActivator = jsDynamicProxyActivator ?? throw new ArgumentNullException(nameof(jsDynamicProxyActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation)
        {
            if (invocation.DefinitionAttributes.IsAttributeDefined(typeof(ReturnDynamicProxyAttribute))) {
                var result = await invocation.GetNonDeterminingResult<IJSObjectReference>();
                var determinedResult = (TValue)jsDynamicProxyActivator.CreateInstance(invocation.TaskArgumentType, result);
                invocation.SetDeterminedResult(new ValueTask<TValue>(determinedResult));
            }
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            ValueTask.CompletedTask;
    }
}
