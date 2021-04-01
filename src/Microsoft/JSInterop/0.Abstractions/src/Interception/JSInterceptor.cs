// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptor : IJSInterceptor
    {
        public static JSInterceptor Default = new JSInterceptor();

        protected virtual IJSInterceptor Interceptor { get; }

        public JSInterceptor() =>
            Interceptor = new DefaultInterceptor();

        public JSInterceptor(IJSInterceptor interceptor) =>
            Interceptor = interceptor ?? throw new System.ArgumentNullException(nameof(interceptor));

        public virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            Interceptor.InterceptInvokeAsync(invocation);

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            Interceptor.InterceptInvokeVoidAsync(invocation);

        private class DefaultInterceptor : IJSInterceptor
        {
            public virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            ValueTask.CompletedTask;

            public ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
                ValueTask.CompletedTask;
        }
    }
}
