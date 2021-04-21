// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public static class YetNullable
    {
        public static bool IsNullable(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(YetNullable<>);

        public static YetNullable<T> Null<T>() =>
            YetNullable<T>.Null;

        public static YetNullable<T> MaybeNull<T>(T? nullableValue)
            where T : struct
        {
            if (nullableValue.HasValue) {
                return new YetNullable<T>(nullableValue.Value);
            }

            return YetNullable<T>.Null;
        }

        public static YetNullable<T> MaybeNull<T>([AllowNull] T nullableValue) =>
            new YetNullable<T>(nullableValue);

        public static bool Equals<T>(YetNullable<T> left, YetNullable<T> right)
        {
            if (left.IsNull && right.IsNull) {
                return true;
            }

            if (left.IsNull || right.IsNull) {
                return false;
            }

            return EqualityComparer<T>.Default.Equals(left.value, right.value);
        }

        public static int Compare<T>(YetNullable<T> left, YetNullable<T> right)
        {
            if (left.IsNull && right.IsNull) {
                return 0;
            }

            if (left.IsNull) {
                return -1;
            }

            if (right.IsNull) {
                return 1;
            }

            return Comparer<T>.Default.Compare(left.value, right.value);
        }

        /// <summary>
        /// Returns the underlying type of <see cref="YetNullable{T}"/>.
        /// If <paramref name="type"/> is not of type 
        /// <see cref="YetNullable{T}"/> null will
        /// be returned. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The underlying type of <see cref="YetNullable{T}"/>.</returns>
        public static Type? GetUnderlyingType(Type type)
        {
            if ((object)type == null) {
                throw new ArgumentNullException(nameof(type));
            }

            var result = default(Type);

            if (type.IsGenericType && !type.IsGenericTypeDefinition) {
                Type genericType = type.GetGenericTypeDefinition();

                if (ReferenceEquals(genericType, typeof(YetNullable<>))) {
                    result = type.GetGenericArguments()[0];
                }
            }

            return result;
        }
    }
}
