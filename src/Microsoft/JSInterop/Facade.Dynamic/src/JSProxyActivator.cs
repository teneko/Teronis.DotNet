using System.Dynamic;
using ImpromptuInterface;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public static class JSProxyActivator
    {
        public static IJSProxy<T> CreateInstance<T>(IJSObjectReference objectReference)
            where T : IJSProxy
        {
            var expandoObject = new ExpandoObject();

            //typeof(IJSProxy<T>).GetMembers(Bindin

            return expandoObject.ActLike<IJSProxy<T>>();
        }
    }
}
