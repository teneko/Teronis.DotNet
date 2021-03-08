using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public readonly struct JSFacades : IAsyncDisposable, IReadOnlyList<IAsyncDisposable>, IJSFacadeResolver
    {
        public int Count =>
            jsFacades.Count;

        private readonly List<IAsyncDisposable> jsFacades;
        private readonly IJSFacadeResolver jsFacadeResolver;

        public JSFacades(List<IAsyncDisposable> jsFacades, IJSFacadeResolver jsFacadeResolver) {
            this.jsFacades = jsFacades ?? throw new ArgumentNullException(nameof(jsFacades));
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
        }

        public JSFacades(IJSFacadeResolver jsFacadeResolver) {
            jsFacades = new List<IAsyncDisposable>();
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
        }

        public IAsyncDisposable this[int index] =>
            jsFacades[index];

        public IEnumerator<IAsyncDisposable> GetEnumerator()
        {
            return jsFacades.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)jsFacades).GetEnumerator();
        }

        public async ValueTask DisposeAsync()
        {
            if (jsFacades is null) {
                return;
            }

            foreach (var moduleWrapper in jsFacades) {
                await moduleWrapper.DisposeAsync();
            }
        }

        public async ValueTask<IAsyncDisposable> ResolveModule(string pathRelativeToWwwRoot, Type jsFacadeType)
        {
            var module = await jsFacadeResolver.ResolveModule(pathRelativeToWwwRoot, jsFacadeType);
            jsFacades.Add(module);
            return module;
        }
    }
}
