// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSObjectInterceptor : IJSObjectInterceptor
    {
        public static JSObjectInterceptor Default = new JSObjectInterceptor();

        protected virtual IJSObjectInterceptor Interceptor { get; }

        public JSObjectInterceptor() =>
            Interceptor = new DefaultInterceptor();

        public JSObjectInterceptor(IJSObjectInterceptor interceptor) =>
            Interceptor = interceptor ?? throw new System.ArgumentNullException(nameof(interceptor));

        public virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            Interceptor.InterceptInvokeAsync(invocation);

        public virtual ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            Interceptor.InterceptInvokeVoidAsync(invocation);

        private class DefaultInterceptor : IJSObjectInterceptor
        {
            public virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            ValueTask.CompletedTask;

            public ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
                ValueTask.CompletedTask;
        }
    }
}
