using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSIndependentFacadesInitializer : IJSIndependentFacadesInitializer
    {
        public async ValueTask<IJSFacades> InitializeFacadesAsync(object component, IJSFacadeResolver jsFacadeResolver)
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

            return new JSFacades(jsFacades, jsFacadeResolver);
        }

        public ValueTask<IJSFacades> InitializeFacadesAsync(IJSFacadeResolver jsFacadeResolver) =>
            new ValueTask<IJSFacades>(new JSFacades(new List<IAsyncDisposable>(), jsFacadeResolver));
    }
}
