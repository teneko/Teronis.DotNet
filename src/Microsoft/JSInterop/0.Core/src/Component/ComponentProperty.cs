// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public sealed class ComponentProperty : ManagedDefinitionBase, IComponentProperty
    {
        public PropertyInfo PropertyInfo =>
            propertyInfo;

        public override Type MemberType =>
            PropertyInfo.PropertyType;

        private readonly PropertyInfo propertyInfo;

        public ComponentProperty(PropertyInfo propertyInfo)
            : base(propertyInfo) =>
            this.propertyInfo = propertyInfo;
    }
}
