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

        #region property

        public static VariableInfo TryGetPropertyVariableInfo(this Type type, string propertyName, VariableInfoSettings settings = null)
        {
            settings = settings ?? VariableInfoSettings.Default;
            var property = type.GetProperty(propertyName, settings.Flags);
            property.TryToVariableInfo(out var varInfo);
            return varInfo;
        }

        public static IEnumerable<VariableInfo> GetPropertyVariableInfos(this Type type, VariableInfoSettings settings = null)
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var property in type.GetProperties(settings.Flags))
                if ((!settings.IncludeIfReadable || property.CanRead)
                    && (!settings.IncludeIfWritable || property.CanWrite)
                    && (settings.IncludeByAttributeTypes == null || settings.IncludeByAttributeTypes.All(attrType => property.IsDefined(attrType, settings.IncludeByAttributeTypesInherit)))
                    && !(settings.ExcludeIfReadable && property.CanRead)
                    && !(settings.ExcludeIfWritable && property.CanWrite)
                    && (!(settings.ExcludeByAttributeTypes != null && settings.ExcludeByAttributeTypes.Any(attrType => property.IsDefined(attrType, settings.ExcludeByAttributeTypesInherit))))
                    && property.TryToVariableInfo(out var varInfo))
                    yield return varInfo;
        }

        public static IEnumerable<VariableInfo> GetPropertyVariableInfos(this Type beginType, Type interruptAtType, VariableInfoSettings settings = null)
            => TypeTools.GetVariableInfos((_type, _flags) => GetPropertyVariableInfos(_type, _flags), beginType, settings, interruptAtType);

        public static AttributeVariableInfo<T> TryGetPropertyVariableInfoByAttribute<T>(this Type type, string propertyName, VariableInfoSettings settings = null, bool? getCustomAttributesInherit = null)
            where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;
            var property = type.GetProperty(propertyName, settings.Flags);
            property.TryToAttributePropertyVariableInfo(out AttributeVariableInfo<T> varInfo, getCustomAttributesInherit);
            return varInfo;
        }

        public static IEnumerable<AttributeVariableInfo<T>> GetPropertyAttributeVariableInfos<T>(this Type targetType, VariableInfoSettings settings = null, Type interruptAtType = null, bool? getCustomAttributesInherit = null)
            where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var type in targetType.GetBaseTypes(interruptAtType))
                foreach (var propertyInfo in type.GetProperties(settings.Flags))
                    if (propertyInfo.TryToAttributePropertyVariableInfo(out AttributeVariableInfo<T> varInfo, getCustomAttributesInherit))
                        yield return varInfo;
        }

        static bool TryToAttributePropertyVariableInfo<T>(this PropertyInfo property, out AttributeVariableInfo<T> varInfo, bool? getCustomAttributesInherit = null)
            where T : Attribute
            => TypeTools.TryToAttributeVariableInfo(property, out varInfo, getCustomAttributesInherit);

        #endregion

        #region field

        public static VariableInfo TryGetFieldVariableInfo(this Type type, string fieldName, VariableInfoSettings settings = null)
        {
            settings = settings ?? VariableInfoSettings.Default;
            var field = type.GetField(fieldName, settings.Flags);
            field.TryToVariableInfo(out var varInfo);
            return varInfo;
        }

        public static IEnumerable<VariableInfo> GetFieldVariableInfos(this Type type, VariableInfoSettings settings = null)
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var field in type.GetFields(settings.Flags))
                if ((settings.IncludeByAttributeTypes == null || settings.IncludeByAttributeTypes.All(attrType => field.IsDefined(attrType, settings.IncludeByAttributeTypesInherit)))
                    && (!(settings.ExcludeByAttributeTypes != null && settings.ExcludeByAttributeTypes.Any(attrType => field.IsDefined(attrType, settings.ExcludeByAttributeTypesInherit))))
                    && field.TryToVariableInfo(out var varInfo))
                    yield return varInfo;
        }

        public static IEnumerable<VariableInfo> GetFieldVariableInfos(this Type beginType, Type interruptAtType, VariableInfoSettings settings = null)
            => TypeTools.GetVariableInfos((type, flags) => GetFieldVariableInfos(type, flags), beginType, settings, interruptAtType);

        public static AttributeVariableInfo<T> TryGetFieldAttributeVariableInfo<T>(this Type type, string fieldName, VariableInfoSettings settings = null, bool? getCustomAttributesInherit = null)
            where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;
            var field = type.GetField(fieldName, settings.Flags);
            field.TryToAttrVarInfo(out AttributeVariableInfo<T> varInfo, getCustomAttributesInherit);
            return varInfo;
        }

        public static IEnumerable<AttributeVariableInfo<T>> GetFieldAttributeVariableInfos<T>(this Type targetType, VariableInfoSettings settings = null, Type interruptAtType = null, bool? getCustomAttributesInherit = null)
            where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var type in targetType.GetBaseTypes(interruptAtType))
                foreach (var fieldInfo in type.GetFields(settings.Flags))
                    if (fieldInfo.TryToAttrVarInfo(out AttributeVariableInfo<T> varInfo, getCustomAttributesInherit))
                        yield return varInfo;
        }

        #endregion

        #region variable

        public static VariableInfo TryGetVariableInfo(this Type type, string varName, VariableInfoSettings settings = null)
            => type.TryGetPropertyVariableInfo(varName, settings) ?? type.TryGetFieldVariableInfo(varName, settings);

        public static IEnumerable<VariableInfo> GetVariableInfos(this Type type, VariableInfoSettings settings = null)
        {
            foreach (var property in type.GetPropertyVariableInfos(settings))
                yield return property;

            foreach (var field in type.GetFieldVariableInfos(settings))
                yield return field;
        }

        public static IEnumerable<VariableInfo> GetVariableInfos(this Type type, Type interruptAtType, VariableInfoSettings settings = null)
        {
            foreach (var property in type.GetPropertyVariableInfos(interruptAtType, settings))
                yield return property;

            foreach (var field in type.GetFieldVariableInfos(interruptAtType, settings))
                yield return field;
        }

        public static AttributeVariableInfo<T> TryGetAttributeVariableInfo<T>(this Type type, string varName, VariableInfoSettings settings = null, bool? getCustomAttributesInherit = null)
            where T : Attribute
            => type.TryGetPropertyVariableInfoByAttribute<T>(varName, settings, getCustomAttributesInherit) ?? type.TryGetFieldAttributeVariableInfo<T>(varName, settings, getCustomAttributesInherit);

        public static IEnumerable<AttributeVariableInfo<T>> GetAttributeVariableInfos<T>(this Type type, VariableInfoSettings settings = null, Type interruptAtType = null, bool? getCustomAttributesInherit = null)
            where T : Attribute
        {
            foreach (var property in GetPropertyAttributeVariableInfos<T>(type, settings, interruptAtType, getCustomAttributesInherit))
                yield return property;

            foreach (var field in GetFieldAttributeVariableInfos<T>(type, settings, interruptAtType, getCustomAttributesInherit))
                yield return field;
        }

        /// <returns>Returns null if passed attribute allows multiple declarations.</returns>
        public static AttributeVariableInfo<T>[] TryGetOrderedAttributeVariableInfos<T>(this Type type, VariableInfoSettings settings = null, Type interruptAtType = null, bool? getCustomAttributesInherit = null)
            where T : Attribute, IZeroBasedIndex
        {
            if (typeof(T).GetCustomAttribute<AttributeUsageAttribute>().AllowMultiple)
                return null;

            var vars = GetAttributeVariableInfos<T>(type, settings, interruptAtType, getCustomAttributesInherit).ToList();
            var array = new AttributeVariableInfo<T>[vars.Count];

            foreach (var variable in vars)
                array[variable.Attributes[0].Index] = variable;

            return array;
        }

        #endregion

        public static IEnumerable<Type> GetBaseTypes(this Type type, Type interruptAtBaseType = null)
        {
            var nextType = type;

            do {
                yield return nextType;
            } while ((nextType = nextType.BaseType) != (interruptAtBaseType ?? type.BaseType));
        }
    }
}