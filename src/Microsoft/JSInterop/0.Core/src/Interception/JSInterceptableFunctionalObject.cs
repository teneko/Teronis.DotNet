// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptableFunctionalObject : JSFunctionalObject, IJSObjectInterceptor
    {
        private readonly IJSObjectInterceptor jsFunctionalObjectInterceptor;

        public JSInterceptableFunctionalObject(IJSObjectInterceptor jsFunctionalObjectInterceptor) =>
            this.jsFunctionalObjectInterceptor = jsFunctionalObjectInterceptor;

        protected override ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            jsFunctionalObjectInterceptor.InterceptInvokeAsync(invocation);

        protected override ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            jsFunctionalObjectInterceptor.InterceptInvokeVoidAsync(invocation);
    }
}
