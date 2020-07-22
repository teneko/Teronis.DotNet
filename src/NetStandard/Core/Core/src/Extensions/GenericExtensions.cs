using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Teronis.Reflection;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class GenericExtensions
    {
        public static bool ReturnNonDefault<T>(this T inValue, [MaybeNull] out T outValue, Func<T>? getNonDefaultIfDefault = null) => 
            TeronisUtils.ReturnNonDefault(inValue, out outValue, getNonDefaultIfDefault);

        public static I ReturnValue<I>(this I inValue) => 
            TeronisUtils.ReturnInValue(inValue);

        public static I ReturnInValue<I>(this I inValue, out I outInValue) => 
            TeronisUtils.ReturnInValue(inValue, out outInValue);

        public static I ReturnInValue<I>(this I inValue, Action<I> mutateInValue) => 
            TeronisUtils.ReturnInValue(inValue, mutateInValue);

        public static I ReturnInValue<I>(this I inValue, Func<I, I> modifyInValue) => 
            TeronisUtils.ReturnInValue(inValue, modifyInValue);

        public static I ReturnInValue<I>(this I inValue, Action doSomething) => 
            TeronisUtils.ReturnInValue(inValue, doSomething);

        public static async Task<I> ReturnInValue<I>(this I inValue, Task task) => 
            await TeronisUtils.ReturnInValue(inValue, task);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, [MaybeNull] out I outInValue, V value) => 
            TeronisUtils.ReturnValue(inValue, out outInValue, value);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, [MaybeNull] out I outInValue, Func<V> getValue) => 
            TeronisUtils.ReturnValue(inValue, out outInValue, getValue);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, [MaybeNull] out I outInValue, Func<I, V> getValue) => 
            TeronisUtils.ReturnValue(inValue, out outInValue, getValue);

        [return: MaybeNull]
        public static V ReturnValue<I, V>(this I inValue, Func<I, V> getValue) => 
            TeronisUtils.ReturnValue(inValue, getValue);

        public static TargetType ShallowCopy<SourceType, TargetType>(this SourceType source)
            where SourceType : notnull
            where TargetType : notnull => 
            TeronisReflectionUtils.ShallowCopy<SourceType, TargetType>(source);

        public static InstanceType ShallowCopy<InstanceType>(this InstanceType instance)
            where InstanceType : notnull => 
            TeronisReflectionUtils.ShallowCopy(instance);
    }
}
