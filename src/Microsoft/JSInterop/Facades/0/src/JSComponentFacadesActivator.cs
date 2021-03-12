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
            functionalFacadesActivator.CreateInstanceAsync(component, jsFacadeResolver);

        public IJSComponentFacades CreateEmptyInstance() =>
            functionalFacadesActivator.CreateEmptyInstance(jsFacadeResolver);
    }
}
