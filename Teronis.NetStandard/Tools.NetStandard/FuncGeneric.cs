using System;

namespace Teronis.Tools.NetStandard
{
    public class FuncGenericTools
    {
        public static bool ReturnIsInvocable<T>(Func<T> getInValue, out T outInValue)
            => GenericTools.ReturnValue(getInValue == null ? default : getInValue(), out outInValue, getInValue != null);
    }
}
