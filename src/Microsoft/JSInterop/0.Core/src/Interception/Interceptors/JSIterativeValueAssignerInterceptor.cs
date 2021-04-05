// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Component.ValueAssigners;
using Teronis.Microsoft.JSInterop.Component.ValueAssigners.Builder;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public class JSIterativeValueAssignerInterceptor : IJSInterceptor
    {
        private readonly IValueAssignerList propertyAssignerList;

        public JSIterativeValueAssignerInterceptor(IValueAssignerList propertyAssignerList) =>
            this.propertyAssignerList = propertyAssignerList ?? throw new ArgumentNullException(nameof(propertyAssignerList));

        public async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            var propertyAssignerContext = new ValueAssignerContext(propertyAssignerList);

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
