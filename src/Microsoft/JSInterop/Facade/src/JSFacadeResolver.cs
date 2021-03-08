using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeResolver : IJSFacadeResolver
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IJSFacadeDictionary jsFacadeDictionary;

        public JSFacadeResolver(IJSRuntime jsRuntime, IJSFacadeDictionary jsFacadeDictionary)
        {
            this.jsRuntime = jsRuntime;
            this.jsFacadeDictionary = jsFacadeDictionary;
        }

        public ValueTask<IJSObjectReference> CreateModuleReferenceAsync(string relativeWwwRootPath) =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", relativeWwwRootPath);

        public virtual async ValueTask<IAsyncDisposable> ResolveModule(string pathRelativeToWwwRoot, Type jsFacadeType)
        {
            if (!jsFacadeDictionary.TryGetValue(jsFacadeType, out var moduleWrapperResolver)) {
                throw new NotSupportedException($"Type {jsFacadeType} is not supported.");
            }

            var module = await CreateModuleReferenceAsync(pathRelativeToWwwRoot);
            return moduleWrapperResolver!.Invoke(module);
        }

        //public ValueTask<IJSObjectReference> CreateObjectReference

        ValueTask<IAsyncDisposable> IJSFacadeResolver.ResolveModule(string relativeWwwRootPath, Type jsFacadeType) =>
            ResolveModule(relativeWwwRootPath, jsFacadeType);
    }
}
