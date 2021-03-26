// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
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

        public static IEnumerable<Type> GetBaseTypes(Type type, Type? interruptingBaseType = null)
        {
            if (type == null) {
                yield break;
            }

            Type? nextType = type;
            var objectType = typeof(object);

            for (; ; ) {
                yield return nextType;
                nextType = nextType.BaseType;

                if (nextType == null || nextType == interruptingBaseType || nextType == objectType) {
                    break;
                }
            }
        }

        public static bool HasDefaultConstructor(Type type)
            => type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;
    }
}
