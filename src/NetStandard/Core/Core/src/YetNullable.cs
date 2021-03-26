// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis
{
    public static class YetNullable
    {
        public static bool IsNullable(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(YetNullable<>);

        public static YetNullable<T> Null<T>() =>
            YetNullable<T>.Null;

        public static YetNullable<T> Null<T>(T? nullableValue)
            where T : struct
        {
            if (nullableValue.HasValue) {
                return new YetNullable<T>(nullableValue.Value);
            }

            return YetNullable<T>.Null;
        }

        public static bool Equals<T>(YetNullable<T> left, YetNullable<T> right)
        {
            if (left.IsNotNull) {
                if (right.IsNotNull) {
                    return EqualityComparer<T>.Default.Equals(left.value, right.value);
                }

                return false;
            }

            if (right.IsNotNull) {
                return false;
            }

            return true;
        }

        public static int Compare<T>(YetNullable<T> left, YetNullable<T> right)
        {
            if (left.IsNotNull) {
                if (right.IsNotNull) {
                    return Comparer<T>.Default.Compare(left.value, right.value);
                }

                return 1;
            }
            if (right.IsNotNull) {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Returns the underlying type of <see cref="YetNullable{T}"/>.
        /// If <paramref name="stillNullableType"/> is not of type 
        /// <see cref="YetNullable{T}"/> <see cref="null"/> will
        /// be returned. 
        /// </summary>
        /// <param name="stillNullableType"></param>
        /// <returns>The underlying type of <see cref="YetNullable{T}"/>.</returns>
        public static Type? GetUnderlyingType(Type stillNullableType)
        {
            if ((object)stillNullableType == null) {
                throw new ArgumentNullException(nameof(stillNullableType));
            }

            var result = default(Type);

            if (stillNullableType.IsGenericType && !stillNullableType.IsGenericTypeDefinition) {
                Type genericType = stillNullableType.GetGenericTypeDefinition();

                if (ReferenceEquals(genericType, typeof(YetNullable<>))) {
                    result = stillNullableType.GetGenericArguments()[0];
                }
            }

            return result;
        }
    }
}
