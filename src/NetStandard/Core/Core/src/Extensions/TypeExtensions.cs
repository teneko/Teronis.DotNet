using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static partial class TypeExtensions
    {
        public static bool IsNullable(this Type type)
            => TypeUtils.IsNullable(type);

        public static object InstantiateUninitializedObject(this Type type)
            => TypeUtils.InstantiateUninitializedObject(type);

        [return: MaybeNull]
        public static T CreateInstanceUninitialized<T>(this Type type)
            => (T)TypeUtils.InstantiateUninitializedObject(type);

        public static object? GetDefault(this Type type)
            => TypeUtils.GetDefault(type);

        public static bool HasInterface<T>(this Type type) => type != null && typeof(T).IsAssignableFrom(type);

        public static bool HasDefaultConstructor(this Type type)
            => type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;

        public static IEnumerable<Type> GetBaseTypes(this Type type, Type? interruptingBaseType = null) =>
            TypeUtils.GetBaseTypes(type, interruptingBaseType);
    }
}
