using System;
using Teronis.Data;

namespace Teronis.Tools
{
    public static class Tools
    {
        /// <summary>
        /// Useful for unsubscribing inline event handlers.
        /// </summary>
        public static WrappedValue<T> ReturnDefaultReplacement<T>(Func<WrappedValue<T>, T> getDefaultValueReplacement)
        {
            var defaultValueWrapper = new WrappedValue<T>(default);
            defaultValueWrapper.Value = getDefaultValueReplacement(defaultValueWrapper);
            return defaultValueWrapper;
        }

        public static int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;
            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
    }
}
