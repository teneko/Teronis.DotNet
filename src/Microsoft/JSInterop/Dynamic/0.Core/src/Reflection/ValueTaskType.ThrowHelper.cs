// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Dynamic.Reflection
{
    partial class ValueTaskType
    {
        internal class ThrowHelper
        {
            public static NotSupportedException CreateNotOfTypeValueTaskException(Type affectedType) =>
                new NotSupportedException($"The type {affectedType} is not {typeof(ValueTask)} or {typeof(ValueTask<>)}.");
        }
    }
}
