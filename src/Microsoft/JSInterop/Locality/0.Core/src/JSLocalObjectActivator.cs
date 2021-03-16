using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSLocalObjectActivator : IJSLocalObjectActivator
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

        public IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference) =>
            new JSLocalObject(getOrBuildJSFunctionalObjectDelegate(), jsObjectReference);

        public async ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName) =>
            CreateInstance(await jsLocalObjectInterop.CreateObjectReferenceAsync(objectName));

        public async ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference objectReference, string objectName)
        {
            var nestedObjectReference = await jsLocalObjectInterop.CreateObjectReferenceAsync(objectReference, objectName);
            return CreateInstance(nestedObjectReference);
        }

        public ValueTask<IJSLocalObject> CreateInstanceAsync(IJSLocalObject jsLocalObject, string objectName) =>
            CreateInstanceAsync(jsLocalObject.JSObjectReference, objectName);
    }
}
