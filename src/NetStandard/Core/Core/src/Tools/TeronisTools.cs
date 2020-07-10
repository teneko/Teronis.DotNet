using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Teronis.Data;

namespace Teronis.Tools
{
    public static class TeronisTools
    {
        public static bool CompareEquality<T>([AllowNull] T one, [AllowNull] T two) =>
            EqualityComparer<T>.Default.Equals(one!, two!);

        public static bool ReturnNonDefault<T>([AllowNull] T inValue, [MaybeNull] out T outValue, Func<T>? getNonDefaultIfDefault = null)
            => !CompareEquality(outValue = inValue, default) || (FuncGenericTools.ReturnIsInvocable(getNonDefaultIfDefault, out outValue) && !CompareEquality(outValue, default));

        [return: MaybeNull]
        public static I ReturnInValue<I>([AllowNull] I inValue, [MaybeNull] out I outInValue)
        {
            outInValue = inValue;
            return inValue;
        }

        [return: MaybeNull]
        public static I ReturnInValue<I>([AllowNull] I inValue, MutateValue<I> mutateInValue)
        {
            mutateInValue(inValue);
            return inValue;
        }

        [return: MaybeNull]
        public static I ReturnInValue<I>([AllowNull] I inValue, ReplaceValueDelegate<I, I> modifyInValue)
            => modifyInValue(inValue);

        [return: MaybeNull]
        public static I ReturnInValue<I>([AllowNull] I inValue, Action doSomething)
        {
            doSomething();
            return inValue;
        }

        public static async Task<I> ReturnInValue<I>([AllowNull] I inValue, Task task)
        {
            await task;
            return inValue!;
        }

        [return: MaybeNull]
        public static V ReturnValue<I, V>([AllowNull] I inValue, [MaybeNull] out I outInValue, [AllowNull] V value)
        {
            outInValue = inValue;
            return value;
        }

        [return: MaybeNull]
        public static V ReturnValue<I, V>([AllowNull] I inValue, [MaybeNull] out I outInValue, Func<V> getValue)
        {
            outInValue = inValue;
            return getValue();
        }

        [return: MaybeNull]
        public static V ReturnValue<I, V>([AllowNull] I inValue, [MaybeNull] out I outInValue, GetInputDelegate<I, V> getValue)
        {
            outInValue = inValue;
            return getValue(inValue);
        }

        [return: MaybeNull]
        public static V ReturnValue<I, V>([AllowNull] I inValue, GetInputDelegate<I, V> getValue)
            => getValue(inValue);

        /// <summary>
        /// Useful for unsubscribing inline event handlers.
        /// </summary>
        public static WrappedValue<T> ReturnDefaultReplacement<T>(Func<WrappedValue<T>, T> getDefaultValueReplacement)
        {
            var defaultValueWrapper = new WrappedValue<T>(default);
            defaultValueWrapper.Value = getDefaultValueReplacement(defaultValueWrapper);
            return defaultValueWrapper;
        }

        [return: MaybeNull]
        public static T ReturnDefaultValueReplacement<T>(Func<WrappedValue<T>, T> getDefaultValueReplacement) =>
            ReturnDefaultReplacement(getDefaultValueReplacement).Value;
    }
}
