using System;
using System.Collections.Generic;
using System.Linq;
using Teronis.Libraries.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class EnumExtensions
	{
        public static IEnumerable<ENUM_TYPE> CrushContainingBitsToEnumerable<ENUM_TYPE>(this ENUM_TYPE enumValue, bool ignoreZero = Library.IgnoreZeroWhenCrushingContainingBits) where ENUM_TYPE : struct, IComparable, IFormattable, IConvertible
        {
            var enumValue2 = enumValue.CastTo<Enum>();
            var enumerator = Enum.GetValues(typeof(ENUM_TYPE)).GetEnumerator();

            bool hasFlag(out ENUM_TYPE enumValue3) => enumValue2.HasFlag((enumValue3 = enumerator.Current.CastTo<ENUM_TYPE>()).CastTo<Enum>());

            if (enumerator.MoveNext() && !ignoreZero && hasFlag(out var enumValue4))
                yield return enumValue4;

            while (enumerator.MoveNext() && hasFlag(out enumValue4))
                yield return enumValue4;
        }

        public static ENUM_TYPE[] CrushContainingBitsToArray<ENUM_TYPE>(this ENUM_TYPE enumValue, bool ignoreZero = Library.IgnoreZeroWhenCrushingContainingBits) where ENUM_TYPE : struct, IComparable, IFormattable, IConvertible
            => CrushContainingBitsToEnumerable(enumValue, ignoreZero).ToArray();

        public static ENUM_TYPE CombineEachEnumValue<ENUM_TYPE>() where ENUM_TYPE : struct, IComparable, IFormattable, IConvertible
        {
            var retVal = 0;

            foreach (var enumVal in Enum.GetValues(typeof(ENUM_TYPE)).Cast<int>())
                retVal |= enumVal;

            return retVal.CastTo<ENUM_TYPE>();
        }
    }
}