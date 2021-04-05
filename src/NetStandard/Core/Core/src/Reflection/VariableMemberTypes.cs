// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Teronis.Reflection
{
    public enum VariableMemberTypes
    {
        Field = MemberTypes.Field,
        Property = MemberTypes.Property,
        FieldAndProperty = Property | Field
    }
}
