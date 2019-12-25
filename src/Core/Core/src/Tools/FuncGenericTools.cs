using System;

namespace Teronis.Tools
{
    public class FuncGenericTools
    {
        public static bool ReturnIsInvocable<T>(Func<T> getInValue, out T outInValue)
            => TeronisTools.ReturnValue(getInValue == null ? default : getInValue(), out outInValue, getInValue != null);
    }
}
