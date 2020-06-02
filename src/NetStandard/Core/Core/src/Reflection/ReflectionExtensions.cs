﻿using System;
using System.Linq;
using System.Reflection;
using Teronis.Reflection;
using Teronis.Tools;

namespace Teronis.Extensions
{
    public static class ReflectionExtensions
    {
        #region Imported from extensions

        public static bool IsVariable(this MemberInfo memberInfo, VariableInfoDescriptor variableInfoDescriptor)
        {
            if (!MemberInfoTools.IsVariable(memberInfo)) {
                return false;
            }

            var isIncludedByAttributeTypes = variableInfoDescriptor.IncludeByAttributeTypes == null || variableInfoDescriptor.IncludeByAttributeTypes.All(attrType => memberInfo.IsDefined(attrType, variableInfoDescriptor.IncludeByAttributeTypesInherit));
            var notExcludedByAttributeTypes = !(variableInfoDescriptor.ExcludeByAttributeTypes != null && variableInfoDescriptor.ExcludeByAttributeTypes.Any(attrType => memberInfo.IsDefined(attrType, variableInfoDescriptor.ExcludeByAttributeTypesInherit)));

            if (memberInfo is FieldInfo fieldInfo) {
                return isIncludedByAttributeTypes && notExcludedByAttributeTypes;
            } else if (memberInfo is PropertyInfo propertyInfo) {
                return (!variableInfoDescriptor.IncludeIfReadable || propertyInfo.CanRead)
                    && (!variableInfoDescriptor.IncludeIfWritable || propertyInfo.CanWrite)
                    && isIncludedByAttributeTypes
                    && !(variableInfoDescriptor.ExcludeIfReadable && propertyInfo.CanRead)
                    && !(variableInfoDescriptor.ExcludeIfWritable && propertyInfo.CanWrite)
                    && notExcludedByAttributeTypes;
            }

            return false;
        }

        private static bool isVariable(this MemberInfo memberInfo, Type attributeType, bool getCustomAttributesInherit)
            => memberInfo.IsDefined(attributeType, getCustomAttributesInherit);

        public static bool IsVariable(this MemberInfo memberInfo, VariableInfoDescriptor descriptor, Type attributeType, bool getCustomAttributesInherit)
            => IsVariable(memberInfo, descriptor) && isVariable(memberInfo, attributeType, getCustomAttributesInherit);

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
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? true; // Library.DefaultCustomAttributesInherit

            if (!MemberInfoTools.IsVariable(memberInfo, typeof(T), _getCustomAttributesInherit)) {
                attrVarInfo = null;
                return false;
            }

            attrVarInfo = GetAttributeVariableMember<T>(memberInfo, getCustomAttributesInherit);
            return true;
        }

        /// <param name="memberInfo">Pass <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>.</param>
        public static bool TryGetAttributeVariableMember(this MemberInfo memberInfo, Type attributeType, out AttributeMemberInfo attrVarInfo, bool? getCustomAttributesInherit = null)
        {
            bool _getCustomAttributesInherit = getCustomAttributesInherit ?? true; // Library.DefaultCustomAttributesInherit

            if (!MemberInfoTools.IsVariable(memberInfo, attributeType, _getCustomAttributesInherit)) {
                attrVarInfo = null;
                return false;
            }

            attrVarInfo = GetAttributeVariableMember(memberInfo, attributeType, getCustomAttributesInherit);
            return true;
        }

        #endregion
    }
}
