using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality.WebAssets
{
    public sealed class JSLocalObjectInterop : IAsyncDisposable, IJSLocalObjectInterop
    {
        private readonly Lazy<Task<IJSObjectReference>> lazyModuleTask;

        public JSLocalObjectInterop(IJSRuntime jsRuntime)
        {
            if (jsRuntime is null) {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            lazyModuleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Teronis.Microsoft.JSInterop.Facades.WebAssets/objectInterop.js").AsTask());
        }

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

        public async ValueTask DisposeAsync()
        {
            if (lazyModuleTask.IsValueCreated) {
                var module = await lazyModuleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
