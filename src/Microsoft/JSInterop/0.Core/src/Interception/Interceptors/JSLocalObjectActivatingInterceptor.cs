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
    /// The interceptor wraps 
    /// </summary>
    public class JSLocalObjectActivatingInterceptor : IJSInterceptor
    {
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSLocalObjectActivatingInterceptor(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            if (invocation.InvocationAttributes.IsAttributeDefined(typeof(ReturnLocalObjectAttribute))) {
                var result = await invocation.GetNonDeterminedResult<IJSObjectReference>();
                var jsLocalObject = await jsLocalObjectActivator.CreateInstanceAsync(result, invocation.Identifier);
                invocation.SetAlternativeResult((TValue)jsLocalObject);
            }
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
