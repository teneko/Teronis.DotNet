// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Teronis.Reflection
{
    public class AttributeMemberInfo<TAttribute> where TAttribute : Attribute
    {
        public Type AttributeType { get; private set; }
        public MemberInfo MemberInfo { get; private set; }
        public List<TAttribute> Attributes { get; private set; }

        protected AttributeMemberInfo(MemberInfo memberInfo, Type attributeType, bool? getCustomAttributesInherit)
        {
            AttributeType = attributeType;
            MemberInfo = memberInfo;
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? true; // Library.DefaultCustomAttributesInherit
            Attributes = new List<TAttribute>();
            var attributes = getAttributes(_getCustomAttributesInherit);
            Attributes.AddRange(attributes);
        }

        public AttributeMemberInfo(MemberInfo memberInfo, bool? getCustomAttributesInherit)
            : this(memberInfo, typeof(TAttribute), getCustomAttributesInherit) { }

        protected virtual IEnumerable<TAttribute> getAttributes(bool getCustomAttributesInherit)
            => MemberInfo.GetCustomAttributes(typeof(TAttribute), getCustomAttributesInherit).Select(x => (TAttribute)x);
    }

    public class AttributeMemberInfo : AttributeMemberInfo<Attribute>
    {
        public AttributeMemberInfo(MemberInfo memberInfo, Type attributeType, bool? getCustomAttributesInherit)
        : base(memberInfo, attributeType, getCustomAttributesInherit) { }

        protected override IEnumerable<Attribute> getAttributes(bool getCustomAttributesInherit)
            => CustomAttributeExtensions.GetCustomAttributes(MemberInfo, AttributeType, getCustomAttributesInherit);
    }
}
