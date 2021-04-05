// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Collections;
using static Teronis.Microsoft.JSInterop.Interception.Interceptors.InterceptionContext;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public sealed class JSIterativeInterceptor : IJSInterceptor
    {
        public IReadOnlyList<IJSInterceptor> Interceptors { get; }

        public JSIterativeInterceptor(IReadOnlyList<IJSInterceptor> interceptors) =>
            Interceptors = interceptors;

        public async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context)
        {
            var subContext = new InterceptionContext(Interceptors);

            await TreeIteratorExecutor<InterceptionEntry>.Default.ExecuteIteratorAsync(
                subContext,
                handler: entry => entry.Item.InterceptInvokeAsync(invocation));
        }

        public async ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context)
        {
            var subContext = new InterceptionContext(Interceptors);

            await TreeIteratorExecutor<InterceptionEntry>.Default.ExecuteIteratorAsync(
                subContext,
                handler: entry => entry.Item.InterceptInvokeVoidAsync(invocation));
        }
    }
}
