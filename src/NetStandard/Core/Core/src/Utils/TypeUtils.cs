// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Reflection;

namespace Teronis.Utils
{
    public static class TypeUtils
    {
        public static bool IsNullable(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static object? GetDefaultOfValueOrReferenceType(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (type.IsValueType) {
                return Instantiator.Instantiate(type);
            }

            return null;
        }

        /// <summary>
        /// Yields the base types of passed type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interruptingBaseType"></param>
        /// <returns>The bases types. (yielded)</returns>
        public static IEnumerable<Type> GetBaseTypes(Type? type, Type? interruptingBaseType = null)
        {
            if (type?.BaseType == null) {
                yield break;
            }

            Type? nextType = type.BaseType;
            var objectType = typeof(object);

            for (; ; ) {
                yield return nextType;
                nextType = nextType.BaseType;

                if (nextType == null || nextType == interruptingBaseType || nextType == objectType) {
                    break;
                }
            }
        }

        /// <summary>
        /// Yields the type that got passed as argument and then yields the base types of passed type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interruptingBaseType"></param>
        /// <returns>The passed type and bases types. (yielded)</returns>
        public static IEnumerable<Type> GetTypeThenBaseTypes(Type type, Type? interruptingBaseType = null)
        {
            if (type == null) {
                yield break;
            }

            yield return type;

            foreach (var baseType in GetBaseTypes(type)) {
                yield return baseType;
            }
        }

        public static bool HasDefaultConstructor(Type type)
            => type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;
    }
}
