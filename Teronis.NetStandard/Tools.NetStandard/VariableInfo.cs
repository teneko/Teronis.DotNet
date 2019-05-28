using System;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Extensions.NetStandard;
using Teronis.Reflection;

namespace Teronis.Tools.NetStandard
{
    public static class VariableInfoTools
    {
        #region members

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
        {
            settings = settings ?? new VariableInfoSettings();

            if (settings.Flags.HasFlag(BindingFlags.DeclaredOnly))
                interruptingBaseType = beginningType.BaseType;
            else {
                settings.Flags |= BindingFlags.DeclaredOnly;
                interruptingBaseType = interruptingBaseType ?? typeof(object);
            }

            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var varInfo in getMembers(type, settings))
                    yield return varInfo;
        }

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType)
            => GetMembers(getMembers, beginningType, interruptingBaseType, default);

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, VariableInfoSettings settings)
            => GetMembers(getMembers, beginningType, default, settings);

        #endregion

        #region attribute members 

        // TYPED

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
        {
            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, settings))
                    if (propertyInfo.TryGetAttributeVariableMember(out AttributeMemberInfo<TAttribute> varAttrInfo, getCustomAttributesInherit))
                        yield return varAttrInfo;
        }

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            where TAttribute: Attribute
            => GetAttributeVariableMembers<TAttribute>(getMembers, beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributeVariableMembers<TAttribute>(getMembers, beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(getMembers, beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(getMembers, beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, VariableInfoSettings settings)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(getMembers, beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, bool? getCustomAttributesInherit)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(getMembers, beginningType, default, default, getCustomAttributesInherit);

        // NON-TYPED

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
        {
            foreach (var type in beginningType.GetBaseTypes(interruptingBaseType))
                foreach (var propertyInfo in GetMembers(getMembers, beginningType, interruptingBaseType, settings))
                    if (propertyInfo.TryGetAttributeVariableMember(attributeType, out var varAttrInfo, getCustomAttributesInherit))
                        yield return varAttrInfo;
        }

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            => GetAttributeVariableMembers(attributeType, getMembers, beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            => GetAttributeVariableMembers(attributeType, getMembers, beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, Type interruptingBaseType)
             => GetAttributeVariableMembers(attributeType, getMembers, beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType)
             => GetAttributeVariableMembers(attributeType, getMembers, beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, VariableInfoSettings settings)
             => GetAttributeVariableMembers(attributeType, getMembers, beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(Type attributeType, Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginningType, bool? getCustomAttributesInherit)
             => GetAttributeVariableMembers(attributeType, getMembers, beginningType, default, default, getCustomAttributesInherit);

        #endregion
    }
}
