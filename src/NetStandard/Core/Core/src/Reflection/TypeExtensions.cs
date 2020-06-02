using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Tools;
using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static partial class TypeExtensions
    {
        #region property

        public static MemberInfo GetPropertyMember(this Type type, string propertyName, VariableInfoDescriptor descriptor = null)
        {
            descriptor = descriptor.DefaultIfNull(true);
            var member = type.GetMember(propertyName, MemberTypes.Property, descriptor.Flags).SingleOrDefault();

            if (member == null || !member.IsVariable(descriptor)) {
                return null;
            }

            return member;
        }

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type type, VariableInfoDescriptor descriptor = null)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));
            descriptor = descriptor.DefaultIfNull(true);

            foreach (MemberInfo property in type.GetProperties(descriptor.Flags)) {
                if (property.IsVariable(descriptor)) {
                    yield return property;
                }
            }
        }

        public static IEnumerable<MemberInfo> GetPropertyMembers(this Type beginningType, Type interruptingBaseType, VariableInfoDescriptor descriptor = null)
            => ReflectionTools.GetMembers((_type, __settings) => GetPropertyMembers(_type, __settings), beginningType, interruptingBaseType, descriptor);

        // // ATTRIBUTES

        // TYPED

        public static AttributeMemberInfo<TAttribute> GetAttributePropertyMember<TAttribute>(this Type type, string propertyName, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
        {
            descriptor = descriptor.DefaultIfNull(true);
            var property = GetPropertyMember(type, propertyName, descriptor);

            if (property == null)
                return null;

            return new AttributeMemberInfo<TAttribute>(property, getCustomAttributesInherit);
        }

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributePropertyMembers<TAttribute>(this Type beginningType, Type interruptingBaseType = null, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
            => ReflectionTools.GetAttributeMembers<TAttribute>(GetPropertyMembers, beginningType, interruptingBaseType, descriptor, getCustomAttributesInherit);

        // NON-TYPED

        public static AttributeMemberInfo GetAttributePropertyMember(this Type type, Type attributeType, string propertyName, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
        {
            descriptor = descriptor.DefaultIfNull(true);
            var property = GetPropertyMember(type, propertyName, descriptor);

            if (property == null) {
                return null;
            }

            return new AttributeMemberInfo(property, attributeType, getCustomAttributesInherit);
        }

        public static IEnumerable<AttributeMemberInfo> GetAttributePropertyMembers(this Type beginningType, Type attributeType, Type interruptingBaseType = null, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            => ReflectionTools.GetAttributeMembers(attributeType, GetPropertyMembers, beginningType, interruptingBaseType, descriptor, getCustomAttributesInherit);

        #endregion

        #region field

        public static MemberInfo GetFieldMember(this Type type, string fieldName, VariableInfoDescriptor descriptor = null)
        {
            descriptor = descriptor.DefaultIfNull(true);
            var member = type.GetMember(fieldName, MemberTypes.Field, descriptor.Flags).SingleOrDefault();

            if (member == null || !member.IsVariable(descriptor))
                return null;

            return member;
        }

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type type, VariableInfoDescriptor descriptor = null)
        {
            descriptor = descriptor.DefaultIfNull(true);

            foreach (MemberInfo field in type.GetFields(descriptor.Flags)) {
                if (field.IsVariable(descriptor)) {
                    yield return field;
                }
            }
        }

        public static IEnumerable<MemberInfo> GetFieldMembers(this Type beginningType, Type interruptingBaseType, VariableInfoDescriptor descriptor = null)
            => ReflectionTools.GetMembers((_type, __settings) => GetFieldMembers(_type, __settings), beginningType, interruptingBaseType, descriptor);

        // // ATTRIBUTES

        // TYPED

        public static AttributeMemberInfo<TAttribute> GetAttributeFieldMember<TAttribute>(this Type type, string fieldName, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
        {
            type = type ?? throw new ArgumentNullException(nameof(type));
            descriptor = descriptor.DefaultIfNull(true);
            var field = GetFieldMember(type, fieldName, descriptor);

            if (field == null) {
                return null;
            }

            return new AttributeMemberInfo<TAttribute>(field, getCustomAttributesInherit);
        }

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeFieldMembers<TAttribute>(this Type beginningType, Type interruptingBaseType = null, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
            => ReflectionTools.GetAttributeMembers<TAttribute>(GetFieldMembers, beginningType, interruptingBaseType, descriptor, getCustomAttributesInherit);

        // NON-TYPED

        public static AttributeMemberInfo GetAttributeFieldMember(this Type type, Type attributeType, string fieldName, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));
            descriptor = descriptor.DefaultIfNull(true);
            var field = GetFieldMember(type, fieldName, descriptor);

            if (field == null)
                return null;

            return new AttributeMemberInfo(field, attributeType, getCustomAttributesInherit);
        }

        public static IEnumerable<AttributeMemberInfo> GetAttributeFieldMembers(this Type beginningType, Type attributeType, Type interruptingBaseType = null, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            => ReflectionTools.GetAttributeMembers(attributeType, GetFieldMembers, beginningType, interruptingBaseType, descriptor, getCustomAttributesInherit);

        #endregion

        #region variable

        public static MemberInfo GetVariableMember(this Type type, string variableName, VariableInfoDescriptor descriptor = null)
            => GetPropertyMember(type, variableName, descriptor) ?? GetFieldMember(type, variableName, descriptor);

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type type, VariableMemberTypes? variableMemberType = null, VariableInfoDescriptor descriptor = null)
        {
            descriptor = descriptor.DefaultIfNull(true);
            var isVariableMemberTypeNull = variableMemberType is null;

            if (isVariableMemberTypeNull || variableMemberType.Value.HasFlag(VariableMemberTypes.Field)) {
                foreach (var variable in GetFieldMembers(type, descriptor)) {
                    yield return variable;
                }
            }

            if (isVariableMemberTypeNull || variableMemberType.Value.HasFlag(VariableMemberTypes.Property)) {
                foreach (var variable in GetPropertyMembers(type, descriptor)) {
                    yield return variable;
                }
            }
        }

        public static IEnumerable<MemberInfo> GetVariableMembers(this Type beginningType, Type interruptingBaseType, VariableMemberTypes? variableMemberType = null, VariableInfoDescriptor descriptor = null)
        {
            descriptor = descriptor.DefaultIfNull(true);
            var isVariableMemberTypeNull = variableMemberType is null;

            if (isVariableMemberTypeNull || variableMemberType.Value.HasFlag(VariableMemberTypes.Field)) {
                foreach (var variable in GetFieldMembers(beginningType, interruptingBaseType, descriptor)) {
                    yield return variable;
                }
            }

            if (isVariableMemberTypeNull || variableMemberType.Value.HasFlag(VariableMemberTypes.Property)) {
                foreach (var variable in GetPropertyMembers(beginningType, interruptingBaseType, descriptor)) {
                    yield return variable;
                }
            }
        }

        // // // // ATTRIBUTES

        // // // NON-ORDERED

        // // NON-BASE-TYPE-LOOP

        // TYPED

        public static AttributeMemberInfo<TAttribute> GetAttributeVariableMember<TAttribute>(this Type type, string variableName, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
            => GetAttributeFieldMember<TAttribute>(type, variableName, descriptor, getCustomAttributesInherit) ?? GetAttributePropertyMember<TAttribute>(type, variableName, descriptor, getCustomAttributesInherit);

        // NON-TYPED

        public static AttributeMemberInfo GetAttributeVariableMember(this Type type, Type attributeType, string variableName, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            => GetAttributeFieldMember(type, attributeType, variableName, descriptor, getCustomAttributesInherit) ?? GetAttributePropertyMember(type, attributeType, variableName, descriptor, getCustomAttributesInherit);

        // // BASE-TYPE-LOOP

        // TYPED

        public static IEnumerable<AttributeMemberInfo<TAttribute>> GetAttributeVariableMembers<TAttribute>(this Type beginningType, Type interruptingBaseType = null, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute
        {
            descriptor = descriptor.DefaultIfNull(true);

            foreach (var variable in GetAttributeFieldMembers<TAttribute>(beginningType, interruptingBaseType, descriptor, getCustomAttributesInherit))
                yield return variable;

            foreach (var variable in GetAttributePropertyMembers<TAttribute>(beginningType, interruptingBaseType, descriptor, getCustomAttributesInherit))
                yield return variable;
        }

        // NON-TYPED

        public static IEnumerable<AttributeMemberInfo> GetAttributeVariableMembers(this Type beginningType, Type attributeType, Type interruptingBaseType = null, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
        {
            descriptor = descriptor.DefaultIfNull(true);

            foreach (var variable in GetAttributeFieldMembers(beginningType, attributeType, interruptingBaseType, descriptor, getCustomAttributesInherit))
                yield return variable;

            foreach (var variable in GetAttributePropertyMembers(beginningType, attributeType, interruptingBaseType, descriptor, getCustomAttributesInherit))
                yield return variable;
        }

        // // // ORDERED

        // // BASE-TYPE-LOOP

        // TYPED

        /// <returns>Returns null if passed attribute allows multiple declarations.</returns>
        public static AttributeMemberInfo<TAttribute>[] TryGetOrderedAttributeMemberInfos<TAttribute>(this Type type, Type interruptingBaseType = null, VariableInfoDescriptor descriptor = null, bool? getCustomAttributesInherit = null)
            where TAttribute : Attribute, IZeroBasedIndex
        {
            if (typeof(TAttribute).GetCustomAttribute<AttributeUsageAttribute>().AllowMultiple)
                return null;

            var vars = GetAttributeVariableMembers<TAttribute>(type, interruptingBaseType, descriptor, getCustomAttributesInherit).ToList();
            var array = new AttributeMemberInfo<TAttribute>[vars.Count];

            foreach (var variable in vars)
                array[variable.FirstAttribute().Index] = variable;

            return array;
        }

        #endregion
    }
}
