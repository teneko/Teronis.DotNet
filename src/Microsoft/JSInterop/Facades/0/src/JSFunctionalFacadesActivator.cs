using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFunctionalFacadesActivator : IJSFunctionalFacadesActivator
    {
        public async ValueTask<IJSComponentFacades> CreateInstanceAsync(object component, IJSFacadeResolver jsFacadeResolver)
        {
            if (component is null) {
                throw new ArgumentNullException(nameof(component));
            }

            var jsFacades = new List<IAsyncDisposable>();

            foreach (var propertyInfo in JSFacadeUtils.GetComponentProperties(component.GetType())) {
                if (!ComponentPropertyUtils.TryGetModulePathRelativeToWwwRoot(propertyInfo, out var pathRelativeToWwwRoot)) {
                    continue;
                }

                var jsModule = await jsFacadeResolver.ResolveModuleAsync(pathRelativeToWwwRoot, propertyInfo.PropertyType);
                propertyInfo.SetValue(component, jsModule);
            }

            return new JSComponentFacades(jsFacades, jsFacadeResolver);
        }

        public IJSComponentFacades CreateEmptyInstance(IJSFacadeResolver jsFacadeResolver) =>
            new JSComponentFacades(new List<IAsyncDisposable>(), jsFacadeResolver);
    }
}
