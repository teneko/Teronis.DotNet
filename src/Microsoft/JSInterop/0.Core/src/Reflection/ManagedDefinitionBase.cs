// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Reflection
{
    public abstract class ManagedDefinitionBase : CustomAttributes, IDefinition
    {
        public abstract Type MemberType { get; }

        public ManagedMemberType ManagedMemberType {
            get {
                if (managedMemberType is null) {
                    managedMemberType = new ManagedMemberType(MemberType);
                }

                return managedMemberType;
            }
        }

        private ManagedMemberType? managedMemberType;

        internal ManagedDefinitionBase(ICustomAttributes customAttributes)
            : base(customAttributes) { }

        internal ManagedDefinitionBase(ICustomAttributes customAttributes, ManagedMemberType managedMemberType)
            : base(customAttributes) =>
            this.managedMemberType = managedMemberType;

        protected ManagedDefinitionBase(ICustomAttributeProvider customAttributeProvider)
            : base(customAttributeProvider) { }

        IDefinitionMemberType IDefinition.DefinitionMemberType =>
            ManagedMemberType;
    }
}
