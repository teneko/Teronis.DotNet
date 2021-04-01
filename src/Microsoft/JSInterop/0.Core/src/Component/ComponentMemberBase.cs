// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public abstract class ComponentMemberBase : ManagedDefinitionBase, IComponentMember
    {
        public ComponentMemberBase(ICustomAttributeProvider customAttributeProvider)
            : base(customAttributeProvider) { }

        public abstract MemberInfo MemberInfo { get; }

        public abstract void SetValue(object? owner, object? value);
    }
}
