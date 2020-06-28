using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Tools
{
    public class FuncGenericTools
    {
        public static bool ReturnIsInvocable<T>(Func<T>? getInValue, [MaybeNull] out T outInValue)
            => TeronisTools.ReturnValue(getInValue == null ? default : getInValue(), out outInValue, getInValue != null);
    }
}
