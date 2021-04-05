// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    /// <summary>
    /// May represent a field-, property- or method-info.
    /// </summary>
    public interface IMemberDefinition : ICustomAttributes
    {
        string Name { get; }
        Type MemberType { get; }
        /// <summary>
        /// May represents the field type, property type return type of this definition.
        /// </summary>
        IMemberTypeInfo MemberTypeInfo { get; }
    }
}
