using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Facades
{
    internal sealed class JSFacades : IJSFacades
    {
        public IReadOnlyList<IAsyncDisposable> JSFacadeList =>
            jsFacades;

        private readonly List<IAsyncDisposable> jsFacades;
        private readonly IJSFacadeResolver jsFacadeResolver;

        public JSFacades(List<IAsyncDisposable> jsFacades, IJSFacadeResolver jsFacadeResolver)
        {
            this.jsFacades = jsFacades ?? throw new ArgumentNullException(nameof(jsFacades));
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
        }

        public JSFacades(IJSFacadeResolver jsFacadeResolver)
        {
            jsFacades = new List<IAsyncDisposable>();
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
        }

        public IAsyncDisposable this[int index] =>
            jsFacades[index];

        public async ValueTask DisposeAsync()
        {
            if (jsFacades is null) {
                return;
            }

            foreach (var moduleWrapper in jsFacades) {
                await moduleWrapper.DisposeAsync();
            }
        }

        public async ValueTask<IJSLocalObject> CreateModuleAsync(string pathRelativeToWwwRoot)
        {
            var module = await jsFacadeResolver.CreateModuleAsync(pathRelativeToWwwRoot);
            jsFacades.Add(module);
            return module;
        }

        public async ValueTask<IAsyncDisposable> ResolveModuleAsync(string pathRelativeToWwwRoot, Type jsFacadeType)
        {
            var module = await jsFacadeResolver.ResolveModuleAsync(pathRelativeToWwwRoot, jsFacadeType);
            jsFacades.Add(module);
            return module;
        }

        public async ValueTask<IJSLocalObject> CreateObjectAsync(string objectName)
        {
            var @object = await jsFacadeResolver.CreateObjectAsync(objectName);
            jsFacades.Add(@object);
            return @object;
        }
    }
}
