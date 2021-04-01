// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Interception
{
    /// <summary>
    /// The interceptor wraps 
    /// </summary>
    public class JSLocalObjectActivatingInterceptor : IJSInterceptor
    {
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSLocalObjectActivatingInterceptor(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public virtual async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation)
        {
            if (invocation.DefinitionAttributes.IsAttributeDefined(typeof(ReturnLocalObjectAttribute))) {
                var result = await invocation.GetNonDeterminingResult<IJSObjectReference>();
                var determinedResult = (ValueTask<TValue>)(object)jsLocalObjectActivator.CreateInstanceAsync(result, invocation.JavaScriptIdentifier);
                invocation.SetDeterminedResult(determinedResult);
            }
        }

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            ValueTask.CompletedTask;
    }
}
