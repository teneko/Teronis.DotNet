// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInvocation<TValue> : IJSFunctionalObjectInvocationBase<ValueTask<TValue>>
    {
        Type GenericTaskArgumentType { get; }

        internal abstract void Clone();
    }
}
