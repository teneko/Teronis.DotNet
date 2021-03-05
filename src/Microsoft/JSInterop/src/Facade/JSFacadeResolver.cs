using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public class JSFacadeResolver : IJSFacadeResolver
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IJSFacadeDictionary moduleWrapperDictionary;

        public JSFacadeResolver(IJSRuntime jsRuntime, IJSFacadeDictionary moduleWrapperDictionary)
        {
            this.jsRuntime = jsRuntime;
            this.moduleWrapperDictionary = moduleWrapperDictionary;
        }

        public ValueTask<IJSObjectReference> ResolveModuleAsync(string relativeWwwRootPath) =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", relativeWwwRootPath);

        protected virtual async ValueTask<IAsyncDisposable> ResolveModule(string relativeWwwRootPath, Type moduleWrapperType)
        {
            if (!moduleWrapperDictionary.TryGetValue(moduleWrapperType, out var moduleWrapperResolver)) {
                throw new NotSupportedException($"Type {moduleWrapperType} is not supported.");
            }

            var module = await ResolveModuleAsync(relativeWwwRootPath);
            return moduleWrapperResolver!.Invoke(module);
        }

        ValueTask<IAsyncDisposable> IJSFacadeResolver.ResolveModuleWrapper(string relativeWwwRootPath, Type moduleWrapperType) =>
            ResolveModule(relativeWwwRootPath, moduleWrapperType);
    }
}
