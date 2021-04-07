// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Component.Assigners;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public class JSIterativeValueAssigningInterceptor : IJSInterceptor
    {
        private readonly IValueAssignerList propertyAssignerList;

        public JSIterativeValueAssigningInterceptor(IValueAssignerList propertyAssignerList) =>
            this.propertyAssignerList = propertyAssignerList ?? throw new ArgumentNullException(nameof(propertyAssignerList));

        public async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            var propertyAssignerContext = new InterceptorValueAssignerContext(propertyAssignerList);
            var result = await invocation.GetDeterminedResult();
            propertyAssignerContext.SetInterceptorOriginatingValueResult(result);

            if (await ValueAssignerIteratorExecutor.TryAssignValueAsync(invocation.InvocationDefinition, propertyAssignerContext)) {
                if (!(propertyAssignerContext.ValueResult.Value is TValue value)) {
                    throw new InvalidCastException($"Cannot cast value to {typeof(TValue)}");
                }

                invocation.SetAlternativeResult(value);
            }
        }

        public ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
