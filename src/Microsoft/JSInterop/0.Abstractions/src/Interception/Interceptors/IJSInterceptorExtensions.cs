// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors
{
    public static class IJSInterceptorExtensions
    {
        public static ValueTask InterceptInvokeAsync<TValue>(this IJSInterceptor interceptor, IJSObjectInvocation<TValue> invocation) =>
            interceptor.InterceptInvokeAsync(invocation, new InterceptionContext(interceptor));

        public static ValueTask InterceptInvokeVoidAsync(this IJSInterceptor interceptor, IJSObjectInvocation invocation) =>
            interceptor.InterceptInvokeVoidAsync(invocation, new InterceptionContext(interceptor));
    }
}
