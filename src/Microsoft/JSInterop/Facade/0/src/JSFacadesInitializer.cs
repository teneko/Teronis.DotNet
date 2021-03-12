using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadesInitializer : IJSFacadesInitializer
    {
        private readonly IJSIndependentFacadesInitializer jsFacadesInitializer;
        private readonly IJSFacadeResolver jsFacadeResolver;

        public JSFacadesInitializer(
            IJSIndependentFacadesInitializer jsFacadesInitializer,
            IJSFacadeResolver jsFacadeResolver)
        {
            this.jsFacadesInitializer = jsFacadesInitializer ?? throw new ArgumentNullException(nameof(jsFacadesInitializer));
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
        }

        public ValueTask<IJSFacades> InitializeFacadesAsync(object component) =>
            jsFacadesInitializer.InitializeFacadesAsync(component, jsFacadeResolver);

        public ValueTask<IJSFacades> InitializeFacadesAsync() =>
            jsFacadesInitializer.InitializeFacadesAsync(jsFacadeResolver);
    }
}
