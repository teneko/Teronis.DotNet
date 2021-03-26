// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Reflection
{
    public static class ITypeInfoFilterExtensions
    {
        public static bool IsBlocked(this ITypeInfoFilter typeInfoFilter, TypeInfo typeInfo) =>
            !typeInfoFilter?.IsAllowed(typeInfo) ?? throw new ArgumentNullException(nameof(typeInfoFilter));
    }
}
