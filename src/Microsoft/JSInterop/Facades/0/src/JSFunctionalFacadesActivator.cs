using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFunctionalFacadesActivator : IJSFunctionalFacadesActivator
    {
        public async ValueTask<IJSFacades> CreateInstanceAsync(object component, IJSFacadeResolver jsFacadeResolver)
        {
            if (component is null) {
                throw new ArgumentNullException(nameof(component));
            }

            var jsFacades = new List<IAsyncDisposable>();

            foreach (var propertyInfo in JSFacadeUtils.GetComponentProperties(component.GetType())) {
                if (!ComponentPropertyUtils.TryGetModulePathRelativeToWwwRoot(propertyInfo, out var pathRelativeToWwwRoot)) {
                    continue;
                }

                var jsModule = await jsFacadeResolver.CreateModuleFacadeAsync(pathRelativeToWwwRoot, propertyInfo.PropertyType);
                propertyInfo.SetValue(component, jsModule);
            }

            return new JSFacades(jsFacades, jsFacadeResolver);
        }

        public IJSFacades CreateEmptyInstance(IJSFacadeResolver jsFacadeResolver) =>
            new JSFacades(new List<IAsyncDisposable>(), jsFacadeResolver);
    }
}
