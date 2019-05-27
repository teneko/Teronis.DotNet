using System;
using System.Linq;
using System.Collections.Generic;
using Teronis.Libraries.NetStandard;
using System.Reflection;

namespace Teronis.Reflection
{
    public class AttributeMemberInfo<T> where T : Attribute
    {
        public MemberInfo MemberInfo { get; private set; }
        public List<T> Attributes { get; private set; }

        public AttributeMemberInfo(MemberInfo memberInfo, bool? getCustomAttributesInherit)
        {
            MemberInfo = memberInfo;
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? Library.DefaultCustomAttributesInherit;
            Attributes = MemberInfo.GetCustomAttributes(typeof(T), _getCustomAttributesInherit).Select(x => (T)x).ToList();
        }
    }
}
