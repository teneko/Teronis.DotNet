// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teronis.Extensions
{
    public static class EnumExtensions
    {
        public const bool IgnoreZeroWhenCrushingContainingBits = false;

        public static IEnumerable<EnumType> CrushContainingBitsToEnumerable<EnumType>(this EnumType enumValue, bool ignoreZero = IgnoreZeroWhenCrushingContainingBits)
            where EnumType : struct, IComparable, IFormattable, IConvertible
        {
            var enumValue2 = (Enum)(object)enumValue;
            var enumerator = Enum.GetValues(typeof(EnumType)).GetEnumerator();

            bool hasFlag(out EnumType enumValue3) =>
                enumValue2.HasFlag((Enum)(object)(enumValue3 = (EnumType)enumerator.Current!));

            if (enumerator.MoveNext() && !ignoreZero && hasFlag(out var enumValue4)) {
                yield return enumValue4;
            }

            while (enumerator.MoveNext() && hasFlag(out enumValue4)) {
                yield return enumValue4;
            }
        }

        public static EnumType[] CrushContainingBitsToArray<EnumType>(this EnumType enumValue, bool ignoreZero = IgnoreZeroWhenCrushingContainingBits)
            where EnumType : struct, IComparable, IFormattable, IConvertible
            => CrushContainingBitsToEnumerable(enumValue, ignoreZero).ToArray();

        public static EnumType CombineEachEnumValue<EnumType>() where EnumType : struct, IComparable, IFormattable, IConvertible
        {
            var retVal = 0;

            foreach (var enumVal in Enum.GetValues(typeof(EnumType)).Cast<int>()) {
                retVal |= enumVal;
            }

            return (EnumType)(object)retVal;
        }
    }
}
