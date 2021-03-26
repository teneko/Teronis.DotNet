// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public abstract class JSFunctionalObjectInterceptor : IJSFunctionalObjectInterceptor
    {
        public abstract ValueTask InterceptInvokeAsync<TValue>(IJSFunctionalObjectInvocation<TValue> invocation);
        public abstract ValueTask InterceptInvokeVoidAsync(IJSFunctionalObjectInvocation invocation);
    }
}
