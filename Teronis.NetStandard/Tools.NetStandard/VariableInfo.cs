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

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, Type interruptAt, VariableInfoSettings settings)
        {
            settings = settings ?? new VariableInfoSettings();

            if (settings.Flags.HasFlag(BindingFlags.DeclaredOnly))
                interruptAt = beginAt.BaseType;
            else {
                settings.Flags |= BindingFlags.DeclaredOnly;
                interruptAt = interruptAt ?? typeof(object);
            }

            foreach (var type in beginAt.GetBaseTypes(interruptAt))
                foreach (var varInfo in getMembers(type, settings))
                    yield return varInfo;
        }

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, Type interruptAt)
            => GetMembers(getMembers, beginAt, interruptAt, default);

        public static IEnumerable<MemberInfo> GetMembers(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, VariableInfoSettings settings)
            => GetMembers(getMembers, beginAt, default, settings);

        #endregion

        #region attribute members 

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeMembers<T>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, Type interruptAt, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute
        {
            foreach (var type in beginAt.GetBaseTypes(interruptAt))
                foreach (var propertyInfo in GetMembers(getMembers, beginAt, interruptAt, settings))
                    if (propertyInfo.TryGetAttributeMember(out AttributeMemberInfo<T> varAttrInfo, getCustomAttributesInherit))
                        yield return varAttrInfo;
        }

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeMembers<T>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, Type interruptAt, VariableInfoSettings settings)
            where T: Attribute
            => GetAttributeMembers<T>(getMembers, beginAt, interruptAt, settings, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeMembers<T>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, Type interruptAt, bool? getCustomAttributesInherit)
            where T : Attribute
            => GetAttributeMembers<T>(getMembers, beginAt, interruptAt, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeMembers<T>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, Type interruptAt)
             where T : Attribute
             => GetAttributeMembers<T>(getMembers, beginAt, interruptAt, default, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeMembers<T>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt)
             where T : Attribute
             => GetAttributeMembers<T>(getMembers, beginAt, default, default, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeMembers<T>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, VariableInfoSettings settings)
             where T : Attribute
             => GetAttributeMembers<T>(getMembers, beginAt, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeMembers<T>(Func<Type, VariableInfoSettings, IEnumerable<MemberInfo>> getMembers, Type beginAt, bool? getCustomAttributesInherit)
             where T : Attribute
             => GetAttributeMembers<T>(getMembers, beginAt, default, default, getCustomAttributesInherit);

        #endregion
    }
}
