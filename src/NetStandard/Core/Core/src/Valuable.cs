// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis
{
    public static class Valuable
    {
        public static bool IsValuable(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Valuable<>);

        public static bool Equals<T>(Valuable<T> left, Valuable<T> right)
        {
            if (left.HasNoValue && right.HasNoValue) {
                return true;
            }

            if (left.HasNoValue || right.HasNoValue) {
                return false;
            }

            return EqualityComparer<T>.Default.Equals(left.value!, right.value!);
        }

        public static int Compare<T>(Valuable<T> left, Valuable<T> right)
        {
            if (left.HasNoValue && right.HasNoValue) {
                return 0;
            }

            if (left.HasNoValue) {
                return -1;
            }

            if (right.HasNoValue) {
                return 1;
            }

            return Comparer<T>.Default.Compare(left.value!, right.value!);
        }

        /// <summary>
        /// Returns the underlying type of <see cref="Valuable{T}"/>.
        /// If <paramref name="type"/> is not of type 
        /// <see cref="Valuable{T}"/> null will
        /// be returned. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The underlying type of <see cref="Valuable{T}"/>.</returns>
        public static Type? GetUnderlyingType(Type type)
        {
            if ((object)type == null) {
                throw new ArgumentNullException(nameof(type));
            }

            var result = default(Type);

            if (type.IsGenericType && !type.IsGenericTypeDefinition) {
                Type genericType = type.GetGenericTypeDefinition();

                if (ReferenceEquals(genericType, typeof(Valuable<>))) {
                    result = type.GetGenericArguments()[0];
                }
            }

            return result;
        }
    }
}
