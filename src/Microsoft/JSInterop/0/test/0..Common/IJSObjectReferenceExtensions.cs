// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.JSObjectReferences;

namespace Teronis.Microsoft.JSInterop
{
    public static class IJSObjectReferenceExtensions
    {
        public static ValueTask InvokeVoidAsync(this IJSFunctionalObject jsFunctionalObject) =>
            jsFunctionalObject.InvokeVoidAsync(new JSEmptyObjectReference(), string.Empty, new object[0]);

        public static ValueTask<TValue> InvokeAsync<TValue>(this IJSFunctionalObject jsFunctionalObject) =>
            jsFunctionalObject.InvokeAsync<TValue>(new JSEmptyObjectReference(), string.Empty, new object[0]);
    }
}
