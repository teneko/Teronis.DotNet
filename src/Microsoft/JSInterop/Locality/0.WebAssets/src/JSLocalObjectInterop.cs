using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Modules;

namespace Teronis.Microsoft.JSInterop.Locality.WebAssets
{
    public sealed class JSLocalObjectInterop : IAsyncDisposable, IJSLocalObjectInterop
    {
        private readonly Lazy<ValueTask<IJSModule>> lazyModuleTask;

        public JSLocalObjectInterop(IJSModuleActivator jsModuleActivator)
        {
            lazyModuleTask = new Lazy<ValueTask<IJSModule>>(() => 
                jsModuleActivator.CreateInstanceAsync("./_content/Teronis.Microsoft.JSInterop.Facades.WebAssets/objectInterop.js"));
        }

        public async ValueTask<IJSObjectReference> GetGlobalObjectReference(string? objectName)
        {
            var module = await lazyModuleTask.Value;

            if (objectName is null || objectName == "window") {
                return await module.InvokeAsync<IJSObjectReference>("getWindow");
            }

            return await module.InvokeAsync<IJSObjectReference>("getGlobalObject", objectName);
        }

        public async ValueTask<IJSObjectReference> GetLocalObjectReference(IJSObjectReference objectReference, string objectName)
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
