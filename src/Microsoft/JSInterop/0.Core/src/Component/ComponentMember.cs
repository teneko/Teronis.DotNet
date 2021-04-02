// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public sealed class ComponentMember : ComponentMemberBase
    {
        /// <summary>
        /// Creates component member from property info or field info.
        /// </summary>
        /// <param name="component">The owner.</param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static ComponentMember Create(object? component, MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo) {
                return new ComponentMember(component, fieldInfo);
            }

            if (memberInfo is PropertyInfo propertyInfo) {
                return new ComponentMember(component, propertyInfo);
            }

            throw new NotSupportedException("Unsupported member type.");
        }

        public override MemberInfo MemberInfo =>
            componentMember.MemberInfo;

        public override Type MemberType =>
            componentMember.MemberType;

        private readonly ComponentMemberBase componentMember;

        public ComponentMember(object? component, PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            componentMember = new ComponentProperty(component, propertyInfo);
        }

        public ComponentMember(object? component, FieldInfo fieldInfo)
            : base(fieldInfo) =>
            componentMember = new ComponentField(component, fieldInfo);

        public override void SetValue(object? value)
        {
            if (!(value is null)) {
                var valueType = value.GetType();

                if (!MemberType.IsAssignableFrom(valueType)) {
                    throw new InvalidOperationException($"Instance of type {valueType} cannot be assigned to member of type {MemberType}");
                }
            }

            componentMember.SetValue(value);
        }

        private class ComponentProperty : ComponentMemberBase
        {
            public override MemberInfo MemberInfo =>
                propertyInfo;

            public override Type MemberType =>
                propertyInfo.PropertyType;

            private readonly object? component;
            private readonly PropertyInfo propertyInfo;

            public ComponentProperty(object? component, PropertyInfo propertyInfo)
                : base(propertyInfo)
            {
                this.component = component;
                this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            }

            public override void SetValue(object? value) =>
                propertyInfo.SetValue(component, value);
        }

        private class ComponentField : ComponentMemberBase
        {
            public override MemberInfo MemberInfo =>
                fieldInfo;

            public override Type MemberType =>
                fieldInfo.FieldType;

            private readonly object? component;
            private readonly FieldInfo fieldInfo;

            public ComponentField(object? component, FieldInfo fieldInfo)
                : base(fieldInfo)
            {
                this.component = component;
                this.fieldInfo = fieldInfo ?? throw new ArgumentNullException(nameof(fieldInfo));
            }

            public override void SetValue(object? value) =>
                fieldInfo.SetValue(component, value);
        }
    }
}
