// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Utils
{
    public static class MathUtils
    {
        public static int ModZeroBasedNumber(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static int ModOneBasedNumber(int x, int m)
        {
            int r = x % m;
            return r <= 0 ? r + m : r;
        }

        public static int ShiftAndWrap(int value, int positions)
        {
            positions &= 0x1F;
            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
    }
}
