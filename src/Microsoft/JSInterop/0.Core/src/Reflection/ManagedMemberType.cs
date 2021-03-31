// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public class ManagedMemberType : CustomAttributes, IDefinitionMemberType
    {
        public Type MemberType { get; }

        internal ManagedMemberType(Type memberType, ICustomAttributes customAttributes)
            : base(customAttributes) =>
            MemberType = memberType ?? throw new ArgumentNullException(nameof(memberType));

        public ManagedMemberType(Type memberType)
            : base(memberType) =>
            MemberType = memberType ?? throw new ArgumentNullException(nameof(memberType));
    }
}
