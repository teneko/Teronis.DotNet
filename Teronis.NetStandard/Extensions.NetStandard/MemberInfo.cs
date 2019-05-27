using System;
using System.Linq;
using System.Reflection;
using Teronis.Libraries.NetStandard;
using Teronis.Reflection;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class MemberInfoExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object owner)
            => MemberInfoTools.GetValue(memberInfo, owner);

        public static void SetValue(this MemberInfo memberInfo, object owner, object value)
            => MemberInfoTools.SetValue(memberInfo, owner, value);

        public static bool IsFieldOrProperty(this MemberInfo memberInfo)
            => memberInfo.MemberType == MemberTypes.Field || memberInfo.MemberType == MemberTypes.Property;

        public static Type VariableType(this MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool IsVariable(this MemberInfo memberInfo)
        {
            if (memberInfo == null || !memberInfo.IsFieldOrProperty())
                return false;

            return true;
        }

        public static bool IsVariable(this MemberInfo memberInfo, VariableInfoSettings settings)
        {
            if (!IsVariable(memberInfo))
                return false;

            var isIncludedByAttributeTypes = settings.IncludeByAttributeTypes == null || settings.IncludeByAttributeTypes.All(attrType => memberInfo.IsDefined(attrType, settings.IncludeByAttributeTypesInherit));
            var notExcludedByAttributeTypes = !(settings.ExcludeByAttributeTypes != null && settings.ExcludeByAttributeTypes.Any(attrType => memberInfo.IsDefined(attrType, settings.ExcludeByAttributeTypesInherit)));

            if (memberInfo is FieldInfo fieldInfo)
                return isIncludedByAttributeTypes && notExcludedByAttributeTypes;
            else if (memberInfo is PropertyInfo propertyInfo)
                return (!settings.IncludeIfReadable || propertyInfo.CanRead)
                    && (!settings.IncludeIfWritable || propertyInfo.CanWrite)
                    && isIncludedByAttributeTypes
                    && !(settings.ExcludeIfReadable && propertyInfo.CanRead)
                    && !(settings.ExcludeIfWritable && propertyInfo.CanWrite)
                    && notExcludedByAttributeTypes;

            return false;
        }

        /// <param name="memberInfo">Pass <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.</param>
        public static AttributeMemberInfo<T> GetAttributeMember<T>(this MemberInfo memberInfo, bool? getCustomAttributesInherit = null) where T : Attribute
        {
            memberInfo = MemberInfoTools.GetCheckedVariable(memberInfo);

            if (!memberInfo.IsDefined(typeof(T), false))
                throw new ArgumentException($"The member has not defined an attribute of type {typeof(T)}");

            return new AttributeMemberInfo<T>(memberInfo, getCustomAttributesInherit);
        }

        /// <param name="memberInfo">Pass <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.</param>
        public static bool TryGetAttributeMember<T>(this MemberInfo memberInfo, out AttributeMemberInfo<T> attrVarInfo, bool? getCustomAttributesInherit = null) where T : Attribute
        {
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? Library.DefaultCustomAttributesInherit;

            if (!memberInfo.IsVariable() || !memberInfo.IsDefined(typeof(T), _getCustomAttributesInherit)) {
                attrVarInfo = null;
                return false;
            }

            attrVarInfo = GetAttributeMember<T>(memberInfo, getCustomAttributesInherit);
            return true;
        }
    }
}
