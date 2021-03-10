using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.WebAssets
{
    public sealed class JSObjectInterop : IAsyncDisposable, IJSObjectInterop
    {
        private readonly Lazy<Task<IJSObjectReference>> lazyModuleTask;

        public JSObjectInterop(IJSRuntime jsRuntime)
        {
            if (jsRuntime is null) {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            lazyModuleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Teronis.Microsoft.JSInterop.Facade.WebAssets/objectInterop.js").AsTask());
        }

        public IJSLocalObject CreateObject(IJSObjectReference objectReference) =>
            new JSLocalObject(objectReference, this);

        /// <summary>
        /// Creates a object reference. If <paramref name="objectName"/> is null
        /// or equals "window" then an object reference to window gets returned.
        /// </summary>
        /// <param name="objectName">The object name.</param>
        /// <returns>A JavaScript object reference.</returns>
        public async ValueTask<IJSObjectReference> CreateObjectReferenceAsync(string? objectName)
        {
            var module = await lazyModuleTask.Value;

            if (objectName is null || objectName == "window") {
                return await module.InvokeAsync<IJSObjectReference>("getWindow");
            }

            return await module.InvokeAsync<IJSObjectReference>("getGlobalObject", objectName);
        }

        public async ValueTask<IJSObjectReference> CreateObjectReferenceAsync(IJSObjectReference objectReference, string objectName)
        {
            var module = await lazyModuleTask.Value;
            var nestedObjectReference = await module.InvokeAsync<IJSObjectReference>("getLocalObject", objectReference, objectName);
            return nestedObjectReference;
        }

        public async ValueTask<IJSLocalObject> CreateObjectAsync(string objectName) =>
            CreateObject(await CreateObjectReferenceAsync(objectName));

        public async ValueTask<IJSLocalObject> CreateObjectAsync(IJSObjectReference objectReference, string objectName)
        {
            var module = await lazyModuleTask.Value;
            var nestedObjectReference = await CreateObjectReferenceAsync(objectReference, objectName);
            return new JSLocalObject(nestedObjectReference, this);
        }

        public ValueTask<IJSLocalObject> CreateObjectAsync(IJSLocalObject jsObject, string objectName) =>
            CreateObjectAsync(jsObject.JSObjectReference, objectName);

        public async ValueTask DisposeAsync()
        {
            if (lazyModuleTask.IsValueCreated) {
                var module = await lazyModuleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
