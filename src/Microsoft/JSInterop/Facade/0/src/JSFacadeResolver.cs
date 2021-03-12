using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.LocalObject;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeResolver : IJSFacadeResolver
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IJSFacadeDictionary jsFacadeDictionary;
        private readonly IJSLocalObjectActivator jsObjectActivator;
        private readonly IServiceProvider serviceProvider;

        public JSFacadeResolver(IJSRuntime jsRuntime, IJSFacadeDictionary jsFacadeDictionary, IJSLocalObjectActivator jsObjectActivator, IServiceProvider serviceProvider)
        {
            this.jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            this.jsFacadeDictionary = jsFacadeDictionary ?? throw new ArgumentNullException(nameof(jsFacadeDictionary));
            this.jsObjectActivator = jsObjectActivator;
            this.serviceProvider = serviceProvider;
        }

        public async ValueTask<IJSLocalObject> CreateModuleAsync(string relativeWwwRootPath)
        {
            var jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", relativeWwwRootPath);
            return jsObjectActivator.CreateLocalObject(jsObjectReference);
        }

        public virtual async ValueTask<IAsyncDisposable> ResolveModuleAsync(string pathRelativeToWwwRoot, Type jsFacadeType)
        {
            if (!jsFacadeDictionary.TryGetValue(jsFacadeType, out var jsFacadeCreatorHandler)) {
                throw new NotSupportedException($"Type {jsFacadeType} is not supported.");
            }

            var jsModule = await CreateModuleAsync(pathRelativeToWwwRoot);

            if (!(jsFacadeCreatorHandler is null)) {
                return jsFacadeCreatorHandler.Invoke(jsModule);
            }

            return (IAsyncDisposable)ActivatorUtilities.CreateInstance(serviceProvider, jsFacadeType, jsModule);
        }

        public ValueTask<IJSLocalObject> CreateObjectAsync(string objectName) =>
            jsObjectActivator.CreateLocalObjectAsync(objectName);
    }
}
