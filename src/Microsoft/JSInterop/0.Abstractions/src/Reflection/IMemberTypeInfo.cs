// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    /// <summary>
    /// May represents the field type of field-info,
    /// property type of property-info or return type of method-info.
    /// </summary>
    public interface IMemberTypeInfo : ICustomAttributes
    {
        Type MemberType { get; }
    }
}
