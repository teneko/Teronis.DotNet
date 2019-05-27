using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Tools.NetStandard;
using Teronis.Reflection;

namespace Teronis.Extensions.NetStandard
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
            => TypeTools.IsNullable(type);

        public static object InstantiateUninitializedObject(this Type type)
            => TypeTools.InstantiateUninitializedObject(type);

        public static object GetDefault(this Type type)
            => TypeTools.GetDefault(type);

        public static bool HasInterface<T>(this Type type) => type != null && typeof(T).IsAssignableFrom(type);

        public static bool HasDefaultConstructor(this Type type)
            => type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;

        public static IEnumerable<Type> GetBaseTypes(this Type type, Type interruptAtBaseType)
        {
            var nextType = type;

            do {
                yield return nextType;
            } while ((nextType = nextType.BaseType) != (interruptAtBaseType ?? type.BaseType));
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
            => GetBaseTypes(type, default);

        #region property

        public static MemberInfo GetPropertyMember(this Type type, string propertyName, VariableInfoSettings settings)
        {
            settings = settings ?? VariableInfoSettings.Default;
            var member = type.GetMember(propertyName, MemberTypes.Property, settings.Flags).First();

            if (member == null || !member.IsVariable(settings))
                return null;

            return member;
        }

        public static MemberInfo GetPropertyMember(this Type type, string propertyName)
            => GetPropertyMember(type, propertyName, default);

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type type, VariableInfoSettings settings)
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (MemberInfo property in type.GetProperties(settings.Flags))
                if (property.IsVariable(settings))
                    yield return property;
        }

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type type)
            => GetPropertyMembers(type, default(VariableInfoSettings));

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type beginType, Type interruptAt, VariableInfoSettings settings)
            => VariableInfoTools.GetMembers((_type, __settings) => GetPropertyMembers(_type, __settings), beginType, interruptAt, settings);

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type beginType, Type interruptAt)
            => GetPropertyMembers(beginType, interruptAt, default);

        // ATTRIBUTES

        public static AttributeMemberInfo<T> GetAttributePropertyMember<T>(this Type type, string propertyName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;
            var property = GetPropertyMember(type, propertyName, settings);

            if (property == null)
                return null;

            return new AttributeMemberInfo<T>(property, getCustomAttributesInherit);
        }

        public static AttributeMemberInfo<T> GetAttributePropertyMember<T>(this Type type, string propertyName, VariableInfoSettings settings)
            where T : Attribute
            => GetAttributePropertyMember<T>(type, propertyName, settings, default);

        public static AttributeMemberInfo<T> GetAttributePropertyMember<T>(this Type type, string propertyName, bool? getCustomAttributesInherit)
        where T : Attribute
        => GetAttributePropertyMember<T>(type, propertyName, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributePropertyMembers<T>(this Type targetType, Type interruptAt, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute
            => VariableInfoTools.GetAttributeMembers<T>(GetPropertyMembers, targetType, interruptAt, settings, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributePropertyMembers<T>(this Type beginAt, Type interruptAt, VariableInfoSettings settings)
            where T : Attribute
            => VariableInfoTools.GetAttributeMembers<T>(GetPropertyMembers, beginAt, interruptAt, settings);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributePropertyMembers<T>(this Type beginAt, Type interruptAt, bool? getCustomAttributesInherit)
            where T : Attribute
            => VariableInfoTools.GetAttributeMembers<T>(GetPropertyMembers, beginAt, interruptAt, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributePropertyMembers<T>(this Type beginAt, Type interruptAt)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetPropertyMembers, beginAt, interruptAt);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributePropertyMembers<T>(this Type beginAt)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetPropertyMembers, beginAt);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributePropertyMembers<T>(this Type beginAt, VariableInfoSettings settings)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetPropertyMembers, beginAt, settings);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributePropertyMembers<T>(this Type beginAt, bool? getCustomAttributesInherit)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetPropertyMembers, beginAt, getCustomAttributesInherit);

        #endregion

        #region field

        public static MemberInfo GetFieldMember(this Type type, string fieldName, VariableInfoSettings settings)
        {
            settings = settings ?? VariableInfoSettings.Default;
            var member = type.GetMember(fieldName, MemberTypes.Field, settings.Flags).First();

            if (member == null || !member.IsVariable(settings))
                return null;

            return member;
        }

        public static MemberInfo GetFieldMember(this Type type, string fieldName)
            => GetFieldMember(type, fieldName, default);

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type type, VariableInfoSettings settings)
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (MemberInfo field in type.GetFields(settings.Flags))
                if (field.IsVariable(settings))
                    yield return field;
        }

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type type)
            => GetFieldMembers(type, default(VariableInfoSettings));

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type beginType, Type interruptAt, VariableInfoSettings settings)
            => VariableInfoTools.GetMembers((_type, __settings) => GetFieldMembers(_type, __settings), beginType, interruptAt, settings);

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type beginType, Type interruptAt)
            => GetFieldMembers(beginType, interruptAt, default);

        // ATTRIBUTES

        public static AttributeMemberInfo<T> GetAttributeFieldMember<T>(this Type type, string fieldName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;
            var field = GetFieldMember(type, fieldName, settings);

            if (field == null)
                return null;

            return new AttributeMemberInfo<T>(field, getCustomAttributesInherit);
        }

        public static AttributeMemberInfo<T> GetAttributeFieldMember<T>(this Type type, string fieldName, VariableInfoSettings settings)
            where T : Attribute
            => GetAttributeFieldMember<T>(type, fieldName, settings, default);

        public static AttributeMemberInfo<T> GetAttributeFieldMember<T>(this Type type, string fieldName, bool? getCustomAttributesInherit)
        where T : Attribute
        => GetAttributeFieldMember<T>(type, fieldName, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeFieldMembers<T>(this Type targetType, Type interruptAt, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute
            => VariableInfoTools.GetAttributeMembers<T>(GetFieldMembers, targetType, interruptAt, settings, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeFieldMembers<T>(this Type beginAt, Type interruptAt, VariableInfoSettings settings)
            where T : Attribute
            => VariableInfoTools.GetAttributeMembers<T>(GetFieldMembers, beginAt, interruptAt, settings);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeFieldMembers<T>(this Type beginAt, Type interruptAt, bool? getCustomAttributesInherit)
            where T : Attribute
            => VariableInfoTools.GetAttributeMembers<T>(GetFieldMembers, beginAt, interruptAt, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeFieldMembers<T>(this Type beginAt, Type interruptAt)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetFieldMembers, beginAt, interruptAt);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeFieldMembers<T>(this Type beginAt)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetFieldMembers, beginAt);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeFieldMembers<T>(this Type beginAt, VariableInfoSettings settings)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetFieldMembers, beginAt, settings);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeFieldMembers<T>(this Type beginAt, bool? getCustomAttributesInherit)
             where T : Attribute
             => VariableInfoTools.GetAttributeMembers<T>(GetFieldMembers, beginAt, getCustomAttributesInherit);

        #endregion

        #region variable

        public static MemberInfo GetVariableMember(this Type type, string variableName, VariableInfoSettings settings)
        {
            settings = settings ?? VariableInfoSettings.Default;
            return GetPropertyMember(type, variableName, settings);
        }

        public static MemberInfo GetVariableMember(this Type type, string variableName)
            => GetVariableMember(type, variableName, default);

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type type, VariableInfoSettings settings)
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var variable in GetFieldMembers(type, settings))
                yield return variable;

            foreach (var variable in GetPropertyMembers(type, settings))
                yield return variable;
        }

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type type)
            => GetVariableMembers(type, default(VariableInfoSettings));

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type beginType, Type interruptAt, VariableInfoSettings settings)
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var variable in GetFieldMembers(beginType, interruptAt, settings))
                yield return variable;

            foreach (var variable in GetPropertyMembers(beginType, interruptAt, settings))
                yield return variable;
        }

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type beginType, Type interruptAt)
            => GetVariableMembers(beginType, interruptAt, default);

        // ATTRIBUTES

        public static AttributeMemberInfo<T> GetAttributeVariableMember<T>(this Type type, string variableName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute
            => GetAttributeFieldMember<T>(type, variableName, settings, getCustomAttributesInherit) ?? GetAttributePropertyMember<T>(type, variableName, settings, getCustomAttributesInherit);

        public static AttributeMemberInfo<T> GetAttributeVariableMember<T>(this Type type, string variableName, VariableInfoSettings settings)
            where T : Attribute
            => GetAttributeVariableMember<T>(type, variableName, settings, default);

        public static AttributeMemberInfo<T> GetAttributeVariableMember<T>(this Type type, string variableName, bool? getCustomAttributesInherit)
        where T : Attribute
        => GetAttributeVariableMember<T>(type, variableName, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeVariableMembers<T>(this Type beginType, Type interruptAt, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var variable in GetAttributeFieldMembers<T>(beginType, interruptAt, settings, getCustomAttributesInherit))
                yield return variable;

            foreach (var variable in GetAttributePropertyMembers<T>(beginType, interruptAt, settings, getCustomAttributesInherit))
                yield return variable;
        }

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeVariableMembers<T>(this Type beginAt, Type interruptAt, VariableInfoSettings settings)
            where T : Attribute
            => GetAttributeVariableMembers<T>(beginAt, interruptAt, settings, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeVariableMembers<T>(this Type beginAt, Type interruptAt, bool? getCustomAttributesInherit)
            where T : Attribute
            => GetAttributeVariableMembers<T>(beginAt, interruptAt, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeVariableMembers<T>(this Type beginAt, Type interruptAt)
             where T : Attribute
             => GetAttributeVariableMembers<T>(beginAt, interruptAt, default, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeVariableMembers<T>(this Type beginAt)
             where T : Attribute
             => GetAttributeVariableMembers<T>(beginAt, default, default, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeVariableMembers<T>(this Type beginAt, VariableInfoSettings settings)
             where T : Attribute
             => GetAttributeVariableMembers<T>(beginAt, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<T>> GetAttributeVariableMembers<T>(this Type beginAt, bool? getCustomAttributesInherit)
             where T : Attribute
             => GetAttributeVariableMembers<T>(beginAt, default, default, getCustomAttributesInherit);

        /// <returns>Returns null if passed attribute allows multiple declarations.</returns>
        public static AttributeMemberInfo<T>[] TryGetOrderedAttributeMemberInfos<T>(this Type type, Type interruptAt, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where T : Attribute, IZeroBasedIndex
        {
            if (typeof(T).GetCustomAttribute<AttributeUsageAttribute>().AllowMultiple)
                return null;

            var vars = GetAttributeVariableMembers<T>(type, interruptAt, settings, getCustomAttributesInherit).ToList();
            var array = new AttributeMemberInfo<T>[vars.Count];

            foreach (var variable in vars)
                array[variable.FirstAttribute().Index] = variable;

            return array;
        }

        public static IEnumerable<AttributeMemberInfo<T>> TryGetOrderedAttributeMemberInfos<T>(this Type beginAt, Type interruptAt, VariableInfoSettings settings)
            where T : Attribute, IZeroBasedIndex
            => TryGetOrderedAttributeMemberInfos<T>(beginAt, interruptAt, settings, default);

        public static IEnumerable<AttributeMemberInfo<T>> TryGetOrderedAttributeMemberInfos<T>(this Type beginAt, Type interruptAt, bool? getCustomAttributesInherit)
            where T : Attribute, IZeroBasedIndex
            => TryGetOrderedAttributeMemberInfos<T>(beginAt, interruptAt, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<T>> TryGetOrderedAttributeMemberInfos<T>(this Type beginAt, Type interruptAt)
             where T : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<T>(beginAt, interruptAt, default, default);

        public static IEnumerable<AttributeMemberInfo<T>> TryGetOrderedAttributeMemberInfos<T>(this Type beginAt)
             where T : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<T>(beginAt, default, default, default);

        public static IEnumerable<AttributeMemberInfo<T>> TryGetOrderedAttributeMemberInfos<T>(this Type beginAt, VariableInfoSettings settings)
             where T : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<T>(beginAt, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<T>> TryGetOrderedAttributeMemberInfos<T>(this Type beginAt, bool? getCustomAttributesInherit)
             where T : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<T>(beginAt, default, default, getCustomAttributesInherit);

        #endregion
    }
}