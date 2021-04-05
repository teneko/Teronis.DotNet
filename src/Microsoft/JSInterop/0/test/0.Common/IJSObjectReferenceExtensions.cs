// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;
using Teronis.Microsoft.JSInterop.ObjectReferences;

namespace Teronis.Microsoft.JSInterop
{
    public static class IJSObjectReferenceExtensions
    {
        public static ValueTask InvokeVoidAsync(this IJSInterceptor jsInterceptor) =>
            jsInterceptor.InvokeVoidAsync(new EmptyObjectReference(), string.Empty, new object[0]);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSInterceptor jsInterceptor) =>
            jsInterceptor.InvokeAsync<TValue>(new EmptyObjectReference(), string.Empty, new object[0]);
    }
}
