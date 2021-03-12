using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public interface IJSDynamicObjectActivator
    {
        T CreateInstance<T>(IJSObjectReference jsObjectReference)
            where T : class, IJSDynamicObject;
    }
}
