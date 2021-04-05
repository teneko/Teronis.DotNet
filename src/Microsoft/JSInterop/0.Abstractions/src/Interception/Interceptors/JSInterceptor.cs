// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public class JSInterceptor : IJSInterceptor
    {
        public static JSInterceptor Default = new JSInterceptor();

        protected virtual IJSInterceptor Interceptor { get; }

        public JSInterceptor() =>
            Interceptor = new DefaultInterceptor();

        public JSInterceptor(IJSInterceptor interceptor) =>
            Interceptor = interceptor ?? throw new System.ArgumentNullException(nameof(interceptor));

        public virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context) =>
            Interceptor.InterceptInvokeAsync(invocation, context);

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
            Interceptor.InterceptInvokeVoidAsync(invocation, context);

        private class DefaultInterceptor : IJSInterceptor
        {
            public virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context) =>
                ValueTask.CompletedTask;

            public ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context) =>
                ValueTask.CompletedTask;
        }
    }
}
