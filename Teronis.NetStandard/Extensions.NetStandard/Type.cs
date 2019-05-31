using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Tools.NetStandard;
using Teronis.Extensions.NetStandard;
using Teronis.Reflection;

namespace Teronis.Extensions.NetStandard
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
            => TypeTools.IsNullable(type);

        public static object InstantiateUninitializedObject(this Type type)
            => TypeTools.InstantiateUninitializedObject(type);

        public static T InstantiateUninitializedObject<T>(this Type type)
            => (T)TypeTools.InstantiateUninitializedObject(type);

        public static object GetDefault(this Type type)
            => TypeTools.GetDefault(type);

        public static bool HasInterface<T>(this Type type) => type != null && typeof(T).IsAssignableFrom(type);

        public static bool HasDefaultConstructor(this Type type)
            => type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;

        public static IEnumerable<Type> GetBaseTypes(this Type type, Type interruptingBaseType)
        {
            var nextType = type;

            do {
                yield return nextType;
            } while ((nextType = nextType.BaseType) != (interruptingBaseType ?? type.BaseType));
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
            => GetBaseTypes(type, default);

        #region property

        public static MemberInfo GetPropertyMember(this Type type, string propertyName, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);
            var member = type.GetMember(propertyName, MemberTypes.Property, settings.Flags).First();

            if (member == null || !member.IsVariable(settings))
                return null;

            return member;
        }

        public static MemberInfo GetPropertyMember(this Type type, string propertyName)
            => GetPropertyMember(type, propertyName, default);

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type type, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);

            foreach (MemberInfo property in type.GetProperties(settings.Flags))
                if (property.IsVariable(settings))
                    yield return property;
        }

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type type)
            => GetPropertyMembers(type, default(VariableInfoSettings));

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            => VariableInfoTools.GetMembers((_type, __settings) => GetPropertyMembers(_type, __settings), beginningType, interruptingBaseType, settings);

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type beginningType, Type interruptingBaseType)
            => GetPropertyMembers(beginningType, interruptingBaseType, default);

        // // ATTRIBUTES

        // TYPED

        public static AttributeMemberInfo<TAttribute> GetAttributePropertyMember<TAttribute>(this Type type, string propertyName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
        {
            settings = settings.DefaultIfNull(true);
            var property = GetPropertyMember(type, propertyName, settings);

            if (property == null)
                return null;

            return new AttributeMemberInfo<TAttribute>(property, getCustomAttributesInherit);
        }

        public static AttributeMemberInfo<TAttribute> GetAttributePropertyMember<TAttribute>(this Type type, string propertyName, VariableInfoSettings settings)
            where TAttribute : Attribute
            => GetAttributePropertyMember<TAttribute>(type, propertyName, settings, default);

        public static AttributeMemberInfo<TAttribute> GetAttributePropertyMember<TAttribute>(this Type type, string propertyName, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributePropertyMember<TAttribute>(type, propertyName, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => VariableInfoTools.GetAttributeVariableMembers<TAttribute>(GetPropertyMembers, beginningType, interruptingBaseType, settings, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            where TAttribute : Attribute
            => GetAttributePropertyMembers<TAttribute>(beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributePropertyMembers<TAttribute>(beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType, Type interruptingBaseType)
             where TAttribute : Attribute
             => GetAttributePropertyMembers<TAttribute>(beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType)
             where TAttribute : Attribute
             => GetAttributePropertyMembers<TAttribute>(beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType, VariableInfoSettings settings)
             where TAttribute : Attribute
             => GetAttributePropertyMembers<TAttribute>(beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType, bool? getCustomAttributesInherit)
             where TAttribute : Attribute
             => GetAttributePropertyMembers<TAttribute>(beginningType, default, default, getCustomAttributesInherit);

        // NON-TYPED

        public static AttributeMemberInfo GetAttributePropertyMember(this Type type, Type attributeType, string propertyName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
        {
            settings = settings.DefaultIfNull(true);
            var property = GetPropertyMember(type, propertyName, settings);

            if (property == null)
                return null;

            return new AttributeMemberInfo(property, attributeType, getCustomAttributesInherit);
        }

        public static AttributeMemberInfo GetAttributePropertyMember(this Type type, Type attributeType, string propertyName, VariableInfoSettings settings)
            => GetAttributePropertyMember(type, attributeType, propertyName, settings, default);

        public static AttributeMemberInfo GetAttributePropertyMember(this Type type, Type attributeType, string propertyName, bool? getCustomAttributesInherit)
            => GetAttributePropertyMember(type, attributeType, propertyName, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            => VariableInfoTools.GetAttributeVariableMembers(attributeType, GetPropertyMembers, beginningType, interruptingBaseType, settings, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, VariableInfoSettings settings)
            => GetAttributePropertyMembers(beginningType, attributeType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            => GetAttributePropertyMembers(beginningType, attributeType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType, Type interruptingBaseType)
             => GetAttributePropertyMembers(beginningType, attributeType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType)
             => GetAttributePropertyMembers(beginningType, attributeType, default, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType, VariableInfoSettings settings)
             => GetAttributePropertyMembers(beginningType, attributeType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType, bool? getCustomAttributesInherit)
             => GetAttributePropertyMembers(beginningType, attributeType, default, default, getCustomAttributesInherit);

        #endregion

        #region field

        public static MemberInfo GetFieldMember(this Type type, string fieldName, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);
            var member = type.GetMember(fieldName, MemberTypes.Field, settings.Flags).First();

            if (member == null || !member.IsVariable(settings))
                return null;

            return member;
        }

        public static MemberInfo GetFieldMember(this Type type, string fieldName)
            => GetFieldMember(type, fieldName, default);

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type type, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);

            foreach (MemberInfo field in type.GetFields(settings.Flags))
                if (field.IsVariable(settings))
                    yield return field;
        }

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type type)
            => GetFieldMembers(type, default(VariableInfoSettings));

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            => VariableInfoTools.GetMembers((_type, __settings) => GetFieldMembers(_type, __settings), beginningType, interruptingBaseType, settings);

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type beginningType, Type interruptingBaseType)
            => GetFieldMembers(beginningType, interruptingBaseType, default);

        // // ATTRIBUTES

        // TYPED

        public static AttributeMemberInfo<TAttribute> GetAttributeFieldMember<TAttribute>(this Type type, string fieldName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
        {
            settings = settings.DefaultIfNull(true);
            var field = GetFieldMember(type, fieldName, settings);

            if (field == null)
                return null;

            return new AttributeMemberInfo<TAttribute>(field, getCustomAttributesInherit);
        }

        public static AttributeMemberInfo<TAttribute> GetAttributeFieldMember<TAttribute>(this Type type, string fieldName, VariableInfoSettings settings)
            where TAttribute : Attribute
            => GetAttributeFieldMember<TAttribute>(type, fieldName, settings, default);

        public static AttributeMemberInfo<T> GetAttributeFieldMember<T>(this Type type, string fieldName, bool? getCustomAttributesInherit)
            where T : Attribute
            => GetAttributeFieldMember<T>(type, fieldName, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeFieldMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => VariableInfoTools.GetAttributeVariableMembers<TAttribute>(GetFieldMembers, beginningType, interruptingBaseType, settings, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeFieldMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            where TAttribute : Attribute
            => GetAttributeFieldMembers<TAttribute>(beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeFieldMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributeFieldMembers<TAttribute>(beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeFieldMembers<TAttribute>(this Type beginningType, Type interruptingBaseType)
             where TAttribute : Attribute
             => GetAttributeFieldMembers<TAttribute>(beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeFieldMembers<TAttribute>(this Type beginningType)
             where TAttribute : Attribute
             => GetAttributeFieldMembers<TAttribute>(beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo<Attribute>> GetAttributeFieldMembers<Attribute>(this Type beginningType, VariableInfoSettings settings)
             where Attribute : System.Attribute
             => GetAttributeFieldMembers<Attribute>(beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeFieldMembers<TAttribute>(this Type beginningType, bool? getCustomAttributesInherit)
             where TAttribute : Attribute
             => GetAttributeFieldMembers<TAttribute>(beginningType, default, default, getCustomAttributesInherit);

        // NON-TYPED



        public static AttributeMemberInfo GetAttributeFieldMember(this Type type, Type attributeType, string fieldName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
        {
            settings = settings.DefaultIfNull(true);
            var field = GetFieldMember(type, fieldName, settings);

            if (field == null)
                return null;

            return new AttributeMemberInfo(field, attributeType, getCustomAttributesInherit);
        }

        public static AttributeMemberInfo GetAttributeFieldMember(this Type type, Type attributeType, string fieldName, VariableInfoSettings settings)
            => GetAttributeFieldMember(type, attributeType, fieldName, settings, default);

        public static AttributeMemberInfo GetAttributeFieldMember(this Type type, Type attributeType, string fieldName, bool? getCustomAttributesInherit)
            => GetAttributeFieldMember(type, attributeType, fieldName, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            => VariableInfoTools.GetAttributeVariableMembers(attributeType, GetFieldMembers, beginningType, interruptingBaseType, settings, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, VariableInfoSettings settings)
            => GetAttributeFieldMembers(beginningType, attributeType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            => GetAttributeFieldMembers(beginningType, attributeType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType, Type interruptingBaseType)
             => GetAttributeFieldMembers(beginningType, attributeType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType)
             => GetAttributeFieldMembers(beginningType, attributeType, default, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType, VariableInfoSettings settings)
             => GetAttributeFieldMembers(beginningType, attributeType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType, bool? getCustomAttributesInherit)
             => GetAttributeFieldMembers(beginningType, attributeType, default, default, getCustomAttributesInherit);

        #endregion

        #region variable

        public static MemberInfo GetVariableMember(this Type type, string variableName, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);
            return GetPropertyMember(type, variableName, settings);
        }

        public static MemberInfo GetVariableMember(this Type type, string variableName)
            => GetVariableMember(type, variableName, default);

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type type, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);

            foreach (var variable in GetFieldMembers(type, settings))
                yield return variable;

            foreach (var variable in GetPropertyMembers(type, settings))
                yield return variable;
        }

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type type)
            => GetVariableMembers(type, default(VariableInfoSettings));

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
        {
            settings = settings.DefaultIfNull(true);

            foreach (var variable in GetFieldMembers(beginningType, interruptingBaseType, settings))
                yield return variable;

            foreach (var variable in GetPropertyMembers(beginningType, interruptingBaseType, settings))
                yield return variable;
        }

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type beginningType, Type interruptingBaseType)
            => GetVariableMembers(beginningType, interruptingBaseType, default);

        // // // // ATTRIBUTES

        // // // NON-ORDERED

        // // NON-BASE-TYPE-LOOP

        // TYPED

        public static AttributeMemberInfo<TAttribute> GetAttributeVariableMember<TAttribute>(this Type type, string variableName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributeFieldMember<TAttribute>(type, variableName, settings, getCustomAttributesInherit) ?? GetAttributePropertyMember<TAttribute>(type, variableName, settings, getCustomAttributesInherit);

        public static AttributeMemberInfo<TAttribute> GetAttributeVariableMember<TAttribute>(this Type type, string variableName, VariableInfoSettings settings)
            where TAttribute : Attribute
            => GetAttributeVariableMember<TAttribute>(type, variableName, settings, default);

        public static AttributeMemberInfo<TAttribute> GetAttributeVariableMember<TAttribute>(this Type type, string variableName, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributeVariableMember<TAttribute>(type, variableName, default, getCustomAttributesInherit);

        // NON-TYPED

        public static AttributeMemberInfo GetAttributeVariableMember(this Type type, Type attributeType, string variableName, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            => GetAttributeFieldMember(type, attributeType, variableName, settings, getCustomAttributesInherit) ?? GetAttributePropertyMember(type, attributeType, variableName, settings, getCustomAttributesInherit);

        public static AttributeMemberInfo GetAttributeVariableMember(this Type type, Type attributeType, string variableName, VariableInfoSettings settings)
            => GetAttributeVariableMember(type, attributeType, variableName, settings, default);

        public static AttributeMemberInfo GetAttributeVariableMember(this Type type, Type attributeType, string variableName, bool? getCustomAttributesInherit)
            => GetAttributeVariableMember(type, attributeType, variableName, default, getCustomAttributesInherit);

        // // BASE-TYPE-LOOP

        // TYPED

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
        {
            settings = settings.DefaultIfNull(true);

            foreach (var variable in GetAttributeFieldMembers<TAttribute>(beginningType, interruptingBaseType, settings, getCustomAttributesInherit))
                yield return variable;

            foreach (var variable in GetAttributePropertyMembers<TAttribute>(beginningType, interruptingBaseType, settings, getCustomAttributesInherit))
                yield return variable;
        }

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            where TAttribute : Attribute
            => GetAttributeVariableMembers<TAttribute>(beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            where TAttribute : Attribute
            => GetAttributeVariableMembers<TAttribute>(beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType, Type interruptingBaseType)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType, VariableInfoSettings settings)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType, bool? getCustomAttributesInherit)
             where TAttribute : Attribute
             => GetAttributeVariableMembers<TAttribute>(beginningType, default, default, getCustomAttributesInherit);

        // NON-TYPED

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
        {
            settings = settings.DefaultIfNull(true);

            foreach (var variable in GetAttributeFieldMembers(beginningType, attributeType, interruptingBaseType, settings, getCustomAttributesInherit))
                yield return variable;

            foreach (var variable in GetAttributePropertyMembers(beginningType, attributeType, interruptingBaseType, settings, getCustomAttributesInherit))
                yield return variable;
        }

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, VariableInfoSettings settings)
            => GetAttributeVariableMembers(beginningType, attributeType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            => GetAttributeVariableMembers(beginningType, attributeType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType, Type interruptingBaseType)
             => GetAttributeVariableMembers(beginningType, attributeType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType)
             => GetAttributeVariableMembers(beginningType, attributeType, default, default, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType, VariableInfoSettings settings)
             => GetAttributeVariableMembers(beginningType, attributeType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType, bool? getCustomAttributesInherit)
             => GetAttributeVariableMembers(beginningType, attributeType, default, default, getCustomAttributesInherit);

        // // // ORDERED

        // // BASE-TYPE-LOOP

        // TYPED

        /// <returns>Returns null if passed attribute allows multiple declarations.</returns>
        public static AttributeMemberInfo<TAttribute>[] TryGetOrderedAttributeMemberInfos<TAttribute>(this Type type, Type interruptingBaseType, VariableInfoSettings settings, bool? getCustomAttributesInherit)
            where TAttribute : Attribute, IZeroBasedIndex
        {
            if (typeof(TAttribute).GetCustomAttribute<AttributeUsageAttribute>().AllowMultiple)
                return null;

            var vars = GetAttributeVariableMembers<TAttribute>(type, interruptingBaseType, settings, getCustomAttributesInherit).ToList();
            var array = new AttributeMemberInfo<TAttribute>[vars.Count];

            foreach (var variable in vars)
                array[variable.FirstAttribute().Index] = variable;

            return array;
        }

        public static IEnumerable<AttributeMemberInfo<TAttribute>> TryGetOrderedAttributeMemberInfos<TAttribute>(this Type beginningType, Type interruptingBaseType, VariableInfoSettings settings)
            where TAttribute : Attribute, IZeroBasedIndex
            => TryGetOrderedAttributeMemberInfos<TAttribute>(beginningType, interruptingBaseType, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> TryGetOrderedAttributeMemberInfos<TAttribute>(this Type beginningType, Type interruptingBaseType, bool? getCustomAttributesInherit)
            where TAttribute : Attribute, IZeroBasedIndex
            => TryGetOrderedAttributeMemberInfos<TAttribute>(beginningType, interruptingBaseType, default, getCustomAttributesInherit);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> TryGetOrderedAttributeMemberInfos<TAttribute>(this Type beginningType, Type interruptingBaseType)
             where TAttribute : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<TAttribute>(beginningType, interruptingBaseType, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> TryGetOrderedAttributeMemberInfos<TAttribute>(this Type beginningType)
             where TAttribute : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<TAttribute>(beginningType, default, default, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> TryGetOrderedAttributeMemberInfos<TAttribute>(this Type beginningType, VariableInfoSettings settings)
             where TAttribute : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<TAttribute>(beginningType, default, settings, default);

        public static IEnumerable<AttributeMemberInfo<TAttribute>> TryGetOrderedAttributeMemberInfos<TAttribute>(this Type beginningType, bool? getCustomAttributesInherit)
             where TAttribute : Attribute, IZeroBasedIndex
             => TryGetOrderedAttributeMemberInfos<TAttribute>(beginningType, default, default, getCustomAttributesInherit);

        #endregion
    }
}