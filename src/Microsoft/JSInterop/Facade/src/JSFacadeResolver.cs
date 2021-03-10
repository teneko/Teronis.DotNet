using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facade.WebAssets;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeResolver : IJSFacadeResolver
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IJSFacadeDictionary jsFacadeDictionary;
        private readonly IJSObjectInterop jsObjectInterop;
        private readonly IServiceProvider serviceProvider;

        public JSFacadeResolver(IJSRuntime jsRuntime, IJSFacadeDictionary jsFacadeDictionary, IJSObjectInterop jsObjectInterop, IServiceProvider serviceProvider)
        {
            this.jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            this.jsFacadeDictionary = jsFacadeDictionary ?? throw new ArgumentNullException(nameof(jsFacadeDictionary));
            this.jsObjectInterop = jsObjectInterop;
            this.serviceProvider = serviceProvider;
        }

        public async ValueTask<IJSLocalObject> CreateModuleReferenceAsync(string relativeWwwRootPath)
        {
            var objectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", relativeWwwRootPath);
            return jsObjectInterop.CreateObject(objectReference);
        }

        public virtual async ValueTask<IAsyncDisposable> ResolveModuleAsync(string pathRelativeToWwwRoot, Type jsFacadeType)
        {
            if (!jsFacadeDictionary.TryGetValue(jsFacadeType, out var jsFacadeFactory)) {
                throw new NotSupportedException($"Type {jsFacadeType} is not supported.");
            }

            var moduleReference = await CreateModuleReferenceAsync(pathRelativeToWwwRoot);

            if (!(jsFacadeFactory is null)) {
                return jsFacadeFactory.Invoke(moduleReference);
            }

            return (IAsyncDisposable)ActivatorUtilities.CreateInstance(serviceProvider, jsFacadeType, moduleReference);
        }

        public ValueTask<IJSLocalObject> CreateObjectAsync(string objectName) =>
            jsObjectInterop.CreateObjectAsync(objectName);
    }
}
