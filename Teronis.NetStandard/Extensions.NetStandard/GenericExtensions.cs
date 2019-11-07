using System;
using System.Threading.Tasks;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class GenericExtensions
    {
        public static bool ReturnNonDefault<T>(this T inValue, out T outValue, Func<T> getNonDefaultIfDefault = null)
            => GenericTools.ReturnNonDefault(inValue, out outValue, getNonDefaultIfDefault);

        public static I ReturnInValue<I>(this I inValue, out I outInValue)
            => GenericTools.ReturnInValue(inValue, out outInValue);

        public static I ReturnInValue<I>(this I inValue, Action<I> modifyInValue)
            => GenericTools.ReturnInValue(inValue, modifyInValue);

        public static I ReturnInValue<I>(this I inValue, Func<I, I> modifyInValue)
            => GenericTools.ReturnInValue(inValue, modifyInValue);

        public static I ReturnInValue<I>(this I inValue, Action doSomething)
            => GenericTools.ReturnInValue(inValue, doSomething);

        public static async Task<I> ReturnInValue<I>(this I inValue, Task task)
            => await GenericTools.ReturnInValue(inValue, task);

        public static V ReturnValue<I, V>(this I inValue, out I outInValue, V value)
            => GenericTools.ReturnValue(inValue, out outInValue, value);

        public static V ReturnValue<I, V>(this I inValue, out I outInValue, Func<V> getValue)
            => GenericTools.ReturnValue(inValue, out outInValue, getValue);

        public static V ReturnValue<I, V>(this I inValue, out I outInValue, Func<I, V> getValue)
            => GenericTools.ReturnValue(inValue, out outInValue, getValue);

        public static V ReturnValue<I, V>(this I inValue, Func<I, V> getValue)
            => GenericTools.ReturnValue(inValue, getValue);

        public static TCloningObject ShallowCopy<TCloningObject, TCopyingObject>(this TCopyingObject copyingObject)
            => GenericTools.ShallowCopy<TCloningObject, TCopyingObject>(copyingObject);

        public static TCloningAndCopyingObject ShallowCopy<TCloningAndCopyingObject>(this TCloningAndCopyingObject copyingObject)
            => GenericTools.ShallowCopy(copyingObject);
    }
}
