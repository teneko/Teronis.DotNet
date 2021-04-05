// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public abstract class MemberDefinitionBase : CustomAttributes, IMemberDefinition
    {
        public abstract string Name { get; }
        public abstract Type MemberType { get; }

        public MemberTypeInfo MemberTypeInfo {
            get {
                if (memberTypeInfo is null) {
                    memberTypeInfo = new MemberTypeInfo(MemberType);
                }

                return memberTypeInfo;
            }
        }

        private MemberTypeInfo? memberTypeInfo;

        internal MemberDefinitionBase(ICustomAttributes customAttributes)
            : base(customAttributes) { }

        internal MemberDefinitionBase(ICustomAttributes customAttributes, MemberTypeInfo memberTypeInfo)
            : base(customAttributes) =>
            this.memberTypeInfo = memberTypeInfo;

        protected MemberDefinitionBase(ICustomAttributeProvider customAttributeProvider)
            : base(customAttributeProvider) { }

        IMemberTypeInfo IMemberDefinition.MemberTypeInfo =>
            MemberTypeInfo;
    }
}
