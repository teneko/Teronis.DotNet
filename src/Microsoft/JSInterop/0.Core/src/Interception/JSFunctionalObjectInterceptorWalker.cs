// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class JSFunctionalObjectInterceptorWalker : IJSFunctionalObjectInterceptor
    {
        private readonly IReadOnlyList<IJSFunctionalObjectInterceptor> interceptors;

        public JSFunctionalObjectInterceptorWalker(IReadOnlyList<IJSFunctionalObjectInterceptor> interceptors) =>
            this.interceptors = interceptors;


        //private ValueTask AwaitInterceptionAsync(IJSFunctionalObjectInvocationBase invocation, ValueTask awaitableInterception) {
        //    if (awaitableInterception.IsCompleted) { 
        //        return
        //    }
        //}

        public async ValueTask WalkThroughInterceptorsAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation)
        {
            //await ProvideAwaitableInterceptorCancellationAsync(invocation, async (awaitableInterceptorCancellation) => {
            foreach (var interception in interceptors) {
                 await interception.InterceptInvokeAsync(invocation);
                
                if (invocation.IsInterceptionStopped) {
                    return;
                }
            }
            //});
        }

        public async ValueTask WalkThroughInterceptorsAsync(IJSFunctionalObjectInvocation invocation)
        {
            foreach (var interception in interceptors) {
                await interception.InterceptInvokeVoidAsync(invocation);

                if (invocation.IsInterceptionStopped) {
                    return;
                }
            }

            return;
        }

        ValueTask IJSFunctionalObjectInterceptor.InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation) =>
            WalkThroughInterceptorsAsync(invocation);

        ValueTask IJSFunctionalObjectInterceptor.InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation) =>
            WalkThroughInterceptorsAsync(invocation);
    }
}
