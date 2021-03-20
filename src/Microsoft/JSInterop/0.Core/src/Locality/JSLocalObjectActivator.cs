using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObjectActivator : IInstanceActivatorBase<IJSLocalObject>, IJSLocalObjectActivator
    {
        private readonly IJSLocalObjectInterop jsLocalObjectInterop;
        private readonly GetOrBuildJSFunctionalObjectDelegate getOrBuildJSFunctionalObjectDelegate;

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop, JSLocalObjectActivatorOptions? options)
        {
            this.jsLocalObjectInterop = jsLocalObjectInterop ?? throw new System.ArgumentNullException(nameof(jsLocalObjectInterop));
            getOrBuildJSFunctionalObjectDelegate = options?.GetOrBuildJSFunctionalObject ?? JSFunctionalObject.GetDefault;
        }

        public JSLocalObjectActivator(IJSLocalObjectInterop jsLocalObjectInterop)
            : this(jsLocalObjectInterop, options: null) { }

        public virtual IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference) =>
            new JSLocalObject(getOrBuildJSFunctionalObjectDelegate(), jsObjectReference);

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName)
        {
            var jsLocalObject = CreateInstance(await jsLocalObjectInterop.GetGlobalObjectReference(objectName));
            DispatchInstanceActicated(jsLocalObject);
            return jsLocalObject;
        }

        public virtual async ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string objectName)
        {
            var nestedObjectReference = await jsLocalObjectInterop.GetLocalObjectReference(jsObjectReference, objectName);
            var jsLocalObject = CreateInstance(nestedObjectReference);
            DispatchInstanceActicated(jsLocalObject);
            return jsLocalObject;
        }
    }
}
