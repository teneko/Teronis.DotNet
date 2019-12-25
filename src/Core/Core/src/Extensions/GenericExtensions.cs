using System;
using System.Threading.Tasks;
using Teronis.Tools;

namespace Teronis.Extensions
{
    public static class GenericExtensions
    {
        public static bool ReturnNonDefault<T>(this T inValue, out T outValue, Func<T> getNonDefaultIfDefault = null)
            => TeronisTools.ReturnNonDefault(inValue, out outValue, getNonDefaultIfDefault);

        public static I ReturnInValue<I>(this I inValue, out I outInValue)
            => TeronisTools.ReturnInValue(inValue, out outInValue);

        public static I ReturnInValue<I>(this I inValue, Action<I> modifyInValue)
            => TeronisTools.ReturnInValue(inValue, modifyInValue);

        public static I ReturnInValue<I>(this I inValue, Func<I, I> modifyInValue)
            => TeronisTools.ReturnInValue(inValue, modifyInValue);

        public static I ReturnInValue<I>(this I inValue, Action doSomething)
            => TeronisTools.ReturnInValue(inValue, doSomething);

        public static async Task<I> ReturnInValue<I>(this I inValue, Task task)
            => await TeronisTools.ReturnInValue(inValue, task);

        public static V ReturnValue<I, V>(this I inValue, out I outInValue, V value)
            => TeronisTools.ReturnValue(inValue, out outInValue, value);

        public static V ReturnValue<I, V>(this I inValue, out I outInValue, Func<V> getValue)
            => TeronisTools.ReturnValue(inValue, out outInValue, getValue);

        public static V ReturnValue<I, V>(this I inValue, out I outInValue, Func<I, V> getValue)
            => TeronisTools.ReturnValue(inValue, out outInValue, getValue);

        public static V ReturnValue<I, V>(this I inValue, Func<I, V> getValue)
            => TeronisTools.ReturnValue(inValue, getValue);

        public static TCloningObject ShallowCopy<TCloningObject, TCopyingObject>(this TCopyingObject copyingObject)
            => ReflectionTools.ShallowCopy<TCloningObject, TCopyingObject>(copyingObject);

        public static TCloningAndCopyingObject ShallowCopy<TCloningAndCopyingObject>(this TCloningAndCopyingObject copyingObject)
            => ReflectionTools.ShallowCopy(copyingObject);
    }
}
