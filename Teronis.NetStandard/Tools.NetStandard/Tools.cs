using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.NetStandard;

namespace Teronis.Tools.NetStandard
{
    public static class Tools
    {
        public static bool CompareEquality<T>(T one, T two) => EqualityComparer<T>.Default.Equals(one, two);

        public static bool ReturnNonDefault<T>(T inValue, out T outValue, Func<T> getNonDefaultIfDefault = null)
            => !CompareEquality(outValue = inValue, default) || (ReturnIsFunctional(getNonDefaultIfDefault, out outValue) && !CompareEquality(outValue, default));

        public static bool ReturnBoolValue<T>(T inValue, out T outInValue, bool boolValue)
        {
            outInValue = inValue;
            return boolValue;
        }

        /// <summary>
        /// Only relevant, when you cannot access <paramref name="inValue"/> in one liners, or when <paramref name="inValue"/> is not well initialized.
        /// </summary>
        public static bool ReturnBoolValue<T>(T inValue, out T outInValue, Func<bool> getBoolValue) 
            => ReturnBoolValue(inValue, out outInValue, getBoolValue());

        /// <summary>
        /// Only relevant, when you cannot access <paramref name="inValue"/> in one liners, or when <paramref name="inValue"/> is not well initialized.
        /// </summary>
        public static bool ReturnBoolValue<T>(T inValue, out T outInValue, Func<T, bool> getBoolValue) 
            => ReturnBoolValue(inValue, out outInValue, getBoolValue(inValue));

        public static bool ReturnIsFunctional<T>(Func<T> getInValue, out T outInValue) 
            => ReturnBoolValue(getInValue == null ? default : getInValue(), out outInValue, getInValue != default);

        public static I ReturnInValue<I>(I inValue, out I outInValue)
        {
            outInValue = inValue;
            return inValue;
        }

        public static T ReturnInValue<T>(T inValue, Action<T> modifyInValue)
        {
            modifyInValue(inValue);
            return inValue;
        }

        public static T ReturnInValue<T>(T inValue, Func<T, T> modifyInValue) 
            => modifyInValue(inValue);

        public static I ReturnInValue<I>(I inValue, Action doSomething)
        {
            doSomething();
            return inValue;
        }

        public static async Task<I> ReturnInValue<I>(I inValue, Task task)
        {
            await task;
            return inValue;
        }

        public static O ReturnReturnValue<I, O>(I inValue, out I outInValue, Func<O> getReturnValue)
        {
            outInValue = inValue;
            return getReturnValue();
        }

        public static O ReturnReturnValue<I, O>(I inValue, Func<I, O> getReturnValue) => getReturnValue(inValue);

        /// <summary>
        /// Useful for unsubscribing inline event handlers.
        /// </summary>
        public static T ReturnDefaultReplacement<T>(Func<ValueWrap<T>, T> getDefaultValueReplacement)
        {
            var defaultValueWrapper = new ValueWrap<T>(default);
            defaultValueWrapper.Value = getDefaultValueReplacement(defaultValueWrapper);
            return defaultValueWrapper.Value;
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
