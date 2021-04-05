// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public abstract class ComponentMemberBase : MemberDefinitionBase, IComponentMember
    {
        public abstract MemberInfo MemberInfo { get; }

        public override string Name => 
            MemberInfo.Name;

        public ComponentMemberBase(ICustomAttributeProvider customAttributeProvider)
            : base(customAttributeProvider) { }

        public abstract void SetValue(object? value);
    }
}
