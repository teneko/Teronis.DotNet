// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class JSIteratingInterceptor : IJSObjectInterceptor
    {
        public IReadOnlyList<IJSObjectInterceptor> Interceptors { get; }

        public JSIteratingInterceptor(IReadOnlyList<IJSObjectInterceptor> interceptors) =>
            Interceptors = interceptors;

        public async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation)
        {
            foreach (var interception in Interceptors) {
                await interception.InterceptInvokeAsync(invocation);

                if (invocation.IsInterceptionStopped) {
                    return;
                }
            }
        }

        public async ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation)
        {
            foreach (var interception in Interceptors) {
                await interception.InterceptInvokeVoidAsync(invocation);

                if (invocation.IsInterceptionStopped) {
                    return;
                }
            }

            return;
        }
    }
}
