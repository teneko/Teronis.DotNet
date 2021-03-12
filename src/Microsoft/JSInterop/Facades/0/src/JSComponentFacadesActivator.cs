using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSComponentFacadesActivator : IJSComponentFacadesActivator
    {
        private readonly IJSFunctionalFacadesActivator functionalFacadesActivator;
        private readonly IJSFacadeResolver jsFacadeResolver;

        public JSComponentFacadesActivator(
            IJSFunctionalFacadesActivator jsFacadesInitializer,
            IJSFacadeResolver jsFacadeResolver)
        {
            this.functionalFacadesActivator = jsFacadesInitializer ?? throw new ArgumentNullException(nameof(jsFacadesInitializer));
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
        }

        public ValueTask<IJSComponentFacades> CreateInstance(object component) =>
            functionalFacadesActivator.CreateFacadesAsync(component, jsFacadeResolver);

        public ValueTask<IJSComponentFacades> CreateEmptyInstance() =>
            functionalFacadesActivator.CreateFacadesAsync(jsFacadeResolver);
    }
}
