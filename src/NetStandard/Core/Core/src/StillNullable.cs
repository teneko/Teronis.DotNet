using System;
using System.Collections.Generic;

namespace Teronis
{
    public static class StillNullable
    {
        public static bool IsNullable(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(StillNullable<>);

        public static StillNullable<T> Null<T>()
            where T : notnull =>
            StillNullable<T>.Null;

        public static StillNullable<T> Null<T>(T? nullableValue)
            where T : struct
        {
            if (nullableValue.HasValue) {
                return new StillNullable<T>(nullableValue.Value);
            }

            return StillNullable<T>.Null;
        }

        public static bool Equals<T>(StillNullable<T> left, StillNullable<T> right)
            where T : notnull
        {
            if (left.HasValue) {
                if (right.HasValue) {
                    return EqualityComparer<T>.Default.Equals(left.value, right.value);
                }

                return false;
            }

            if (right.HasValue) {
                return false;
            }

            return true;
        }

        public static int Compare<T>(StillNullable<T> left, StillNullable<T> right)
            where T : notnull
        {
            if (left.HasValue) {
                if (right.HasValue) {
                    return Comparer<T>.Default.Compare(left.value, right.value);
                }

                return 1;
            }
            if (right.HasValue) {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Returns the underlying type of <see cref="StillNullable{T}"/>.
        /// If <paramref name="stillNullableType"/> is not of type 
        /// <see cref="StillNullable{T}"/> <see cref="null"/> will
        /// be returned. 
        /// </summary>
        /// <param name="stillNullableType"></param>
        /// <returns>The underlying type of <see cref="StillNullable{T}"/>.</returns>
        public static Type? GetUnderlyingType(Type stillNullableType)
        {
            if ((object)stillNullableType == null) {
                throw new ArgumentNullException(nameof(stillNullableType));
            }

            var result = default(Type);

            if (stillNullableType.IsGenericType && !stillNullableType.IsGenericTypeDefinition) {
                Type genericType = stillNullableType.GetGenericTypeDefinition();

                if (ReferenceEquals(genericType, typeof(StillNullable<>))) {
                    result = stillNullableType.GetGenericArguments()[0];
                }
            }

            return result;
        }
    }
}
