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
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static ComponentMember Create(MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo) {
                return new ComponentMember(fieldInfo);
            }

            if (memberInfo is PropertyInfo propertyInfo) {
                return new ComponentMember(propertyInfo);
            }

            throw new NotSupportedException("Unsupported member type.");
        }

        public override MemberInfo MemberInfo =>
            componentMember.MemberInfo;

        public override Type MemberType =>
            componentMember.MemberType;

        private readonly ComponentMemberBase componentMember;

        public ComponentMember(PropertyInfo propertyInfo)
            : base(propertyInfo) =>
            componentMember = new ComponentProperty(propertyInfo);

        public ComponentMember(FieldInfo fieldInfo)
            : base(fieldInfo) =>
            componentMember = new ComponentField(fieldInfo);

        public override void SetValue(object? owner, object? value)
        {
            if (!(value is null)) {
                var valueType = value.GetType();

                if (!MemberType.IsAssignableFrom(valueType)) {
                    throw new InvalidOperationException($"Instance of type {valueType} cannot be assigned to member of type {MemberType}");
                }
            }

            componentMember.SetValue(owner, value);
        }

        private class ComponentProperty : ComponentMemberBase
        {
            public override MemberInfo MemberInfo =>
                propertyInfo;

            public override Type MemberType =>
                propertyInfo.PropertyType;

            private readonly PropertyInfo propertyInfo;

            public ComponentProperty(PropertyInfo propertyInfo)
                : base(propertyInfo) =>
                this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

            public override void SetValue(object? owner, object? value) =>
                propertyInfo.SetValue(owner, value);
        }

        private class ComponentField : ComponentMemberBase
        {
            public override MemberInfo MemberInfo =>
                fieldInfo;

            public override Type MemberType =>
                fieldInfo.FieldType;

            private readonly FieldInfo fieldInfo;

            public ComponentField(FieldInfo propertyInfo)
                : base(propertyInfo) =>
                this.fieldInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

            public override void SetValue(object? owner, object? value) =>
                fieldInfo.SetValue(owner, value);
        }
    }
}
