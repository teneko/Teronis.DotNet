using System;
using System.Linq;
using System.Collections.Generic;
using Teronis.Libraries.NetStandard;
using System.Reflection;

namespace Teronis.Reflection
{
    public class AttributeMemberInfo<T> where T : Attribute
    {
        public Type AttributeType { get; private set; }
        public MemberInfo MemberInfo { get; private set; }
        public List<T> Attributes { get; private set; }

        protected AttributeMemberInfo(MemberInfo memberInfo, Type attributeType, bool? getCustomAttributesInherit)
        {
            AttributeType = attributeType;
            MemberInfo = memberInfo;
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? Library.DefaultCustomAttributesInherit;
            Attributes = new List<T>();
            var attributes = getAttributes(_getCustomAttributesInherit);
            Attributes.AddRange(attributes);
        }

        public AttributeMemberInfo(MemberInfo memberInfo, bool? getCustomAttributesInherit)
            : this(memberInfo, typeof(T), getCustomAttributesInherit) { }

        protected virtual IEnumerable<T> getAttributes(bool getCustomAttributesInherit)
            => MemberInfo.GetCustomAttributes(typeof(T), getCustomAttributesInherit).Select(x => (T)x);
    }

    public class AttributeMemberInfo : AttributeMemberInfo<Attribute>
    {
        public AttributeMemberInfo(MemberInfo memberInfo, Type attributeType, bool? getCustomAttributesInherit)
        : base(memberInfo, attributeType, getCustomAttributesInherit) { }

        protected override IEnumerable<Attribute> getAttributes(bool getCustomAttributesInherit)
            => CustomAttributeExtensions.GetCustomAttributes(MemberInfo, AttributeType, getCustomAttributesInherit);
    }
}
