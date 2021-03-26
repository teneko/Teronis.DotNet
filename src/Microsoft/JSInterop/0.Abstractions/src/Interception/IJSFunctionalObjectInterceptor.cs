// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInterceptor
    {
        ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation);
        ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation);
    }
}
