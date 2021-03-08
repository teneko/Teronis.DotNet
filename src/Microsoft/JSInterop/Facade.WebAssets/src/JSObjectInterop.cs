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

        public IJSLocalObjectReference CreateObjectReference(IJSObjectReference objectReference) =>
            new JSLocalObjectReference(objectReference, this);

        /// <summary>
        /// Creates a object reference. If <paramref name="objectName"/> is null
        /// or equals "window" then an object reference to window gets returned.
        /// </summary>
        /// <param name="objectName">The object name.</param>
        /// <returns>A JavaScript object reference.</returns>
        public async ValueTask<IJSLocalObjectReference> CreateObjectReferenceAsync(string? objectName)
        {
            var module = await lazyModuleTask.Value;
            IJSObjectReference objectReference;

            if (objectName is null || objectName == "window") {
                objectReference = await module.InvokeAsync<IJSObjectReference>("getWindow");
            } else {
                objectReference = await module.InvokeAsync<IJSObjectReference>("getGlobalObject", objectName);
            }

            return CreateObjectReference(objectReference);
        }

        public async ValueTask<IJSLocalObjectReference> CreateObjectReferenceAsync(IJSObjectReference objectReference, string objectName)
        {
            var module = await lazyModuleTask.Value;
            var nestedObjectReference = await module.InvokeAsync<IJSObjectReference>("getLocalObject", objectReference, objectName);
            return CreateObjectReference(nestedObjectReference);
        }

        public async ValueTask DisposeAsync()
        {
            if (lazyModuleTask.IsValueCreated) {
                var module = await lazyModuleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
