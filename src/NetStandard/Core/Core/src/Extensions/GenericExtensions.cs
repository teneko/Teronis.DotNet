using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Teronis.Tools;

namespace Teronis.Extensions
{
    public static class GenericExtensions
    {
        public static bool ReturnNonDefault<T>(this T inValue, [MaybeNull] out T outValue, Func<T>? getNonDefaultIfDefault = null)
            => TeronisTools.ReturnNonDefault(inValue, out outValue, getNonDefaultIfDefault);

        [return: MaybeNull]
        public static I ReturnInValue<I>(this I inValue, [MaybeNull] out I outInValue)
            => TeronisTools.ReturnInValue(inValue, out outInValue);

        [return: MaybeNull]
        public static I ReturnInValue<I>(this I inValue, MutateValue<I> mutateInValue)
            => TeronisTools.ReturnInValue(inValue, mutateInValue);

        [return: MaybeNull]
        public static I ReturnInValue<I>(this I inValue, ReplaceValueDelegate<I,I> modifyInValue)
            => TeronisTools.ReturnInValue(inValue, modifyInValue);

        [return: MaybeNull]
        public static I ReturnInValue<I>(this I inValue, Action doSomething)
            => TeronisTools.ReturnInValue(inValue, doSomething);

        public static async Task<I> ReturnInValue<I>(this I inValue, Task task)
            => await TeronisTools.ReturnInValue(inValue, task);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, [MaybeNull] out I outInValue, V value)
            => TeronisTools.ReturnValue(inValue, out outInValue, value);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, [MaybeNull] out I outInValue, Func<V> getValue)
            => TeronisTools.ReturnValue(inValue, out outInValue, getValue);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, [MaybeNull] out I outInValue, GetInputDelegate<I, V> getValue)
            => TeronisTools.ReturnValue(inValue, out outInValue, getValue);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, GetInputDelegate<I, V> getValue)
            => TeronisTools.ReturnValue(inValue, getValue);

        public static TCloningObject ShallowCopy<TCloningObject, TCopyingObject>(this TCopyingObject copyingObject)
            => ReflectionTools.ShallowCopy<TCloningObject, TCopyingObject>(copyingObject);

        public static TCloningAndCopyingObject ShallowCopy<TCloningAndCopyingObject>(this TCloningAndCopyingObject copyingObject)
            => ReflectionTools.ShallowCopy(copyingObject);
    }
}
