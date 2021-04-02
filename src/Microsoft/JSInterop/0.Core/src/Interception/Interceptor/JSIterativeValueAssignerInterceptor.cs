// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Component;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor
{
    public class JSIterativeValueAssignerInterceptor : IJSInterceptor
    {
        private readonly IValueAssignerList propertyAssignerList;

        public JSIterativeValueAssignerInterceptor(IValueAssignerList propertyAssignerList) =>
            this.propertyAssignerList = propertyAssignerList ?? throw new ArgumentNullException(nameof(propertyAssignerList));

        public async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            var propertyAssignerContext = new ValueAssignerContext(propertyAssignerList);

            if (await ValueAssignerIteratorExecutor.TryAssignValueAsync(invocation.Definition, propertyAssignerContext)) {
                invocation.SetAlternativeResult((TValue)propertyAssignerContext.ValueResult.Value!);
            }
        }

        public ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            ValueTask.CompletedTask;
    }
}
