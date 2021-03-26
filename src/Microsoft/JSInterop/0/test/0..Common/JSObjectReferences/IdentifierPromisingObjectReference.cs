// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.JSObjectReferences
{
    public class IdentifierPromisingObjectReference : JSEmptyObjectReference
    {
        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            new ValueTask<TValue>((TValue)(object)identifier);
    }
}
