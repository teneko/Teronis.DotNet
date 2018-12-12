using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Teronis.NetStandard.Tools;

namespace Teronis.NetStandard.Extensions
{
    public static class TypeExtensions
    {
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
                if (!(settings.ExcludeIfReadable && property.CanRead)
                    && !(settings.ExcludeIfWritable && property.CanWrite)
                    && (!settings.RequireReadability || property.CanRead)
                    && (!settings.RequireWritablity || property.CanWrite)
                    && property.TryToVariableInfo(out var varInfo))
                    yield return varInfo;
        }

        public static IEnumerable<VariableInfo> GetPropertyVariableInfos(this Type beginType, VariableInfoSettings settings = null, Type interruptAtType = null)
            => TypeTools.GetVariableInfos((_type, _flags) => GetPropertyVariableInfos(_type, _flags), beginType, settings, interruptAtType);

        public static AttributeVariableInfo<T> TryGetPropertyVariableInfoByAttribute<T>(this Type type, string propertyName, VariableInfoSettings settings = null, bool inherit = Constants.Inherit) where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;
            var property = type.GetProperty(propertyName, settings.Flags);
            property.TryToAttributePropertyVariableInfo(out AttributeVariableInfo<T> varInfo, inherit);
            return varInfo;
        }

        public static IEnumerable<AttributeVariableInfo<T>> GetAttributePropertyVariableInfos<T>(this Type targetType, VariableInfoSettings settings = null, bool inherit = Constants.Inherit, Type interruptAtType = null) where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var type in targetType.GetBaseTypes(interruptAtType))
                foreach (var propertyInfo in type.GetProperties(settings.Flags))
                    if (propertyInfo.TryToAttributePropertyVariableInfo(out AttributeVariableInfo<T> varInfo, inherit))
                        yield return varInfo;
        }

        static bool TryToAttributePropertyVariableInfo<T>(this PropertyInfo property, out AttributeVariableInfo<T> varInfo, bool inherit = Constants.Inherit) where T : Attribute
            => TypeTools.TryToAttributeVariableInfo(property, out varInfo, inherit);

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
                if (/* TODO: Take settings into account. */ field.TryToVariableInfo(out var varInfo))
                    yield return varInfo;
        }

        public static IEnumerable<VariableInfo> GetFieldVariableInfos(this Type beginType, VariableInfoSettings settings = null, Type interruptAtType = null)
            => TypeTools.GetVariableInfos((type, flags) => GetFieldVariableInfos(type, flags), beginType, settings, interruptAtType);

        public static AttributeVariableInfo<T> TryGetFieldAttributeVariableInfo<T>(this Type type, string fieldName, VariableInfoSettings settings = null, bool inherit = Constants.Inherit) where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;
            var field = type.GetField(fieldName, settings.Flags);
            field.TryToAttrVarInfo(out AttributeVariableInfo<T> varInfo, inherit);
            return varInfo;
        }

        public static IEnumerable<AttributeVariableInfo<T>> GetFieldAttributeVariableInfos<T>(this Type targetType, VariableInfoSettings settings = null, bool inherit = Constants.Inherit, Type interruptAtType = null) where T : Attribute
        {
            settings = settings ?? VariableInfoSettings.Default;

            foreach (var type in targetType.GetBaseTypes(interruptAtType))
                foreach (var fieldInfo in type.GetFields(settings.Flags))
                    if (fieldInfo.TryToAttrVarInfo(out AttributeVariableInfo<T> varInfo, inherit))
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

        public static IEnumerable<VariableInfo> GetVariableInfos(this Type type, VariableInfoSettings settings = null, Type interruptAtType = null)
        {
            foreach (var property in type.GetPropertyVariableInfos(settings, interruptAtType))
                yield return property;

            foreach (var field in type.GetFieldVariableInfos(settings, interruptAtType))
                yield return field;
        }

        public static AttributeVariableInfo<T> TryGetAttributeVariableInfo<T>(this Type type, string varName, VariableInfoSettings settings = null, bool inherit = Constants.Inherit) where T : Attribute
            => type.TryGetPropertyVariableInfoByAttribute<T>(varName, settings, inherit) ?? type.TryGetFieldAttributeVariableInfo<T>(varName, settings, inherit);

        public static IEnumerable<AttributeVariableInfo<T>> GetAttributeVariableInfos<T>(this Type type, VariableInfoSettings settings = null, bool inherit = Constants.Inherit, Type interruptAtType = null) where T : Attribute
        {
            foreach (var property in GetAttributePropertyVariableInfos<T>(type, settings, inherit, interruptAtType))
                yield return property;

            foreach (var field in GetFieldAttributeVariableInfos<T>(type, settings, inherit, interruptAtType))
                yield return field;
        }
        
        /// <returns>Returns null if passed attribute allows multiple declarations.</returns>
        public static AttributeVariableInfo<T>[] TryGetOrderedAttributeVariableInfos<T>(this Type type, VariableInfoSettings settings = null, bool inherit = Constants.Inherit) where T : Attribute, IZeroBasedIndex
        {
            if (typeof(T).GetCustomAttribute<AttributeUsageAttribute>().AllowMultiple)
                return null;

            var vars = GetAttributeVariableInfos<T>(type, settings, inherit).ToList();
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

        public static bool HasDefaultConstructor(this Type type) => type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;

        public static object InstantiateUninitializedObject(this Type type) => typeof(Instantiator<>).MakeGenericType(type).GetMethod(nameof(Instantiator<object>.Instantiate), BindingFlags.Public | BindingFlags.Static).Invoke(null, null);

        public static bool HasInterface<T>(this Type type) => type != null && typeof(T).IsAssignableFrom(type);
    }
}