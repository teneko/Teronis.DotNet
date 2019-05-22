using System;
using System.Threading.Tasks;

namespace Teronis.Tools.NetStandard
{
    public class GenericTools
    {
        public static bool ReturnNonDefault<T>(T inValue, out T outValue, Func<T> getNonDefaultIfDefault = null)
            => !Tools.CompareEquality(outValue = inValue, default) || (FuncGenericTools.ReturnIsInvocable(getNonDefaultIfDefault, out outValue) && !Tools.CompareEquality(outValue, default));

        public static I ReturnInValue<I>(I inValue, out I outInValue)
        {
            outInValue = inValue;
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Action<I> modifyInValue)
        {
            modifyInValue(inValue);
            return inValue;
        }

        public static I ReturnInValue<I>(I inValue, Func<I, I> modifyInValue)
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
        
        public static V ReturnValue<I, V>(I inValue, out I outInValue, V value)
        {
            outInValue = inValue;
            return value;
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<V> getValue)
        {
            outInValue = inValue;
            return getValue();
        }

        public static V ReturnValue<I, V>(I inValue, out I outInValue, Func<I, V> getValue)
        {
            outInValue = inValue;
            return getValue(inValue);
        }

        public static V ReturnValue<I, V>(I inValue, Func<I, V> getValue)
            => getValue(inValue);
    }
}
