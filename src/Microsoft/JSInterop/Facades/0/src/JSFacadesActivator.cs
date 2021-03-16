using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadesActivator : IJSFacadesActivator
    {
        private readonly IJSFunctionalFacadesActivator functionalFacadesActivator;
        private readonly IJSFacadeResolver jsFacadeResolver;

        public JSFacadesActivator(
            IJSFunctionalFacadesActivator jsFacadesInitializer,
            IJSFacadeResolver jsFacadeResolver)
        {
            this.functionalFacadesActivator = jsFacadesInitializer ?? throw new ArgumentNullException(nameof(jsFacadesInitializer));
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
        }

        public ValueTask<IJSFacades> CreateInstanceAsync(object component) =>
            functionalFacadesActivator.CreateInstanceAsync(component, jsFacadeResolver);

        public IJSFacades CreateInstance() =>
            functionalFacadesActivator.CreateEmptyInstance(jsFacadeResolver);
    }
}
