// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Teronis.Reflection
{
    public interface ITypeInfoFilter
    {
        bool IsAllowed(TypeInfo? typeInfo);
    }
}
