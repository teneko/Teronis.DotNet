using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality.Dynamic
{
    public interface IJSDynamicLocalObjectActivator : IJSLocalObjectActivator
    {
        ValueTask<T> CreateDynamicInstanceAsync<T>(string objectName)
            where T : class, IJSLocalObject;

        ValueTask<T> CreateDynamicInstanceAsync<T>(IJSObjectReference jsObjectReference, string objectName)
            where T : class, IJSLocalObject;

        ValueTask<T> CreateDynamicInstanceAsync<T>(IJSLocalObject jsLocalObject, string objectName)
            where T : class, IJSLocalObject;
    }
}
