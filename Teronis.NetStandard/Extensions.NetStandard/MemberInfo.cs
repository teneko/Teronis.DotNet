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

        private static bool isVariable(this MemberInfo memberInfo, Type attributeType, bool getCustomAttributesInherit)
            => memberInfo.IsDefined(attributeType, getCustomAttributesInherit);

        public static bool IsVariable(this MemberInfo memberInfo, Type attributeType, bool getCustomAttributesInherit)
            => IsVariable(memberInfo) && isVariable(memberInfo, attributeType, getCustomAttributesInherit);

        public static bool IsVariable(this MemberInfo memberInfo, VariableInfoSettings settings, Type attributeType, bool getCustomAttributesInherit)
            => IsVariable(memberInfo, settings) && isVariable(memberInfo, attributeType, getCustomAttributesInherit);

        /// <param name="memberInfo">Pass <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.</param>
        public static AttributeMemberInfo<T> GetAttributeVariableMember<T>(this MemberInfo memberInfo, bool? getCustomAttributesInherit = null)
            where T : Attribute
        {
            MemberInfoTools.CheckedAttributeVariable(memberInfo, typeof(T));
            return new AttributeMemberInfo<T>(memberInfo, getCustomAttributesInherit);
        }

        /// <param name="memberInfo">Pass <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.</param>
        public static AttributeMemberInfo GetAttributeVariableMember(this MemberInfo memberInfo, Type attributeType, bool? getCustomAttributesInherit = null)
        {
            MemberInfoTools.CheckedAttributeVariable(memberInfo, attributeType);
            return new AttributeMemberInfo(memberInfo, attributeType, getCustomAttributesInherit);
        }

        /// <param name="memberInfo">Pass <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.</param>
        public static bool TryGetAttributeVariableMember<T>(this MemberInfo memberInfo, out AttributeMemberInfo<T> attrVarInfo, bool? getCustomAttributesInherit = null)
            where T : Attribute
        {
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? Library.DefaultCustomAttributesInherit;

            if (!IsVariable(memberInfo, typeof(T), _getCustomAttributesInherit)) {
                attrVarInfo = null;
                return false;
            }

            attrVarInfo = GetAttributeVariableMember<T>(memberInfo, getCustomAttributesInherit);
            return true;
        }

        /// <param name="memberInfo">Pass <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.</param>
        public static bool TryGetAttributeVariableMember(this MemberInfo memberInfo, Type attributeType, out AttributeMemberInfo attrVarInfo, bool? getCustomAttributesInherit = null)
        {
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? Library.DefaultCustomAttributesInherit;

            if (!IsVariable(memberInfo, attributeType, _getCustomAttributesInherit)) {
                attrVarInfo = null;
                return false;
            }

            attrVarInfo = GetAttributeVariableMember(memberInfo, attributeType, getCustomAttributesInherit);
            return true;
        }
    }
}
