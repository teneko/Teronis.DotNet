// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public interface ICustomAttributes
    {
        ILookup<Type, Attribute> Attributes { get; }

        bool IsAttributeDefined(Type attributeType);
        bool IsAttributeDefined<T>();

        bool TryGetAttribute<T>([MaybeNullWhen(false)] out T attribute)
            where T : Attribute;
    }
}
