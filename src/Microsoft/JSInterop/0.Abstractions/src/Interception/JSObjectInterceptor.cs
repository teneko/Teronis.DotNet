// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSObjectInterceptor : IJSObjectInterceptor
    {
        public virtual ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation) =>
            ValueTask.CompletedTask;

        public ValueTask InterceptInvokeVoidAsync(IJSObjectInvocation invocation) =>
            ValueTask.CompletedTask;
    }
}
