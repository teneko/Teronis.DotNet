// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Utils
{
    public class FuncGenericUtils
    {
        public static bool IsInvocable<T>(Func<T>? getInValue, [MaybeNull] out T outInValue)
            => TeronisUtils.ReturnValue(getInValue == null ? default! : getInValue(), out outInValue, getInValue != null);
    }
}
