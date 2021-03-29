// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class JSFunctionalObjectInterceptorWalker : IJSObjectInterceptor
    {
        private readonly IReadOnlyList<IJSObjectInterceptor> interceptors;

        public JSFunctionalObjectInterceptorWalker(IReadOnlyList<IJSObjectInterceptor> interceptors) =>
            this.interceptors = interceptors;

        public async ValueTask WalkThroughInterceptorsAsync<TValue>(IJSObjectInvocation<TValue> invocation)
        {
            foreach (var interception in interceptors) {
                await interception.InterceptInvokeAsync(invocation);

                if (invocation.IsInterceptionStopped) {
                    return;
                }
            }
        }

        public async ValueTask WalkThroughInterceptorsAsync(IJSObjectInvocation invocation)
        {
            foreach (var interception in interceptors) {
                await interception.InterceptInvokeVoidAsync(invocation);

                if (invocation.IsInterceptionStopped) {
                    return;
                }
            }

            return;
        }

        ValueTask IJSObjectInterceptor.InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            WalkThroughInterceptorsAsync(invocation);

        ValueTask IJSObjectInterceptor.InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            WalkThroughInterceptorsAsync(invocation);
    }
}
