// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor
{
    public interface IJSInterceptor
    {
        ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation, InterceptionContext context);
        ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation, InterceptionContext context);
    }
}
