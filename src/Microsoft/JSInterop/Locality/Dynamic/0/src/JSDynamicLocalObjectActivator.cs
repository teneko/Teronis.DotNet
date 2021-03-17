using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop.Locality.Dynamic
{
    public class JSDynamicLocalObjectActivator : IJSDynamicLocalObjectActivator
    {
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;
        private readonly IJSDynamicProxyActivator jSDynamicProxyActivator;

        public JSDynamicLocalObjectActivator(IJSLocalObjectActivator jsLocalObjectActivator, IJSDynamicProxyActivator jSDynamicProxyActivator)
        {
            this.jsLocalObjectActivator = jsLocalObjectActivator;
            this.jSDynamicProxyActivator = jSDynamicProxyActivator;
        }

        public IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference) =>
            jsLocalObjectActivator.CreateInstance(jsObjectReference);

        public ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName) =>
            jsLocalObjectActivator.CreateInstanceAsync(objectName);

        public ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string objectName) =>
            jsLocalObjectActivator.CreateInstanceAsync(jsObjectReference, objectName);

        public ValueTask<IJSLocalObject> CreateInstanceAsync(IJSLocalObject jsObjectReference, string objectName) =>
            jsLocalObjectActivator.CreateInstanceAsync(jsObjectReference, objectName);

        public virtual async ValueTask<T> CreateDynamicInstanceAsync<T>(string moduleNameOrPath)
            where T : class, IJSLocalObject
        {
            var jsLocalObject = await CreateInstanceAsync(moduleNameOrPath);
            var jsDynamicLocalObject = jSDynamicProxyActivator.CreateInstance<T>(jsLocalObject);
            return jsDynamicLocalObject;
        }

        public virtual async ValueTask<T> CreateDynamicInstanceAsync<T>(IJSObjectReference jsObjectReference, string objectName)
            where T : class, IJSLocalObject
        {
            var jsLocalObject = await CreateInstanceAsync(jsObjectReference, objectName);
            var jsDynamicLocalObject = jSDynamicProxyActivator.CreateInstance<T>(jsLocalObject);
            return jsDynamicLocalObject;
        }

        public virtual async ValueTask<T> CreateDynamicInstanceAsync<T>(IJSLocalObject jsLocalObject, string objectName)
               where T : class, IJSLocalObject
        {
            var nestedJSLocalObject = await CreateInstanceAsync(jsLocalObject, objectName);
            var jsDynamicLocalObject = jSDynamicProxyActivator.CreateInstance<T>(nestedJSLocalObject);
            return jsDynamicLocalObject;
        }
    }
}
