// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public sealed class EmptyCustomAttributeLookup : ICustomAttributes
    {
        public static EmptyCustomAttributeLookup Instance = new EmptyCustomAttributeLookup();

        public ILookup<Type, Attribute> Attributes { get; }

        public EmptyCustomAttributeLookup() =>
            Attributes = Enumerable.Empty<Attribute>().ToLookup(x => default(Type)!);

        public bool IsAttributeDefined(Type attributeType) =>
            false;

        public bool IsAttributeDefined<T>() =>
            false;

        public bool TryGetAttribtue<T>([MaybeNullWhen(false)] out T attribute)
            where T : Attribute =>
            throw new NotImplementedException();
    }
}
