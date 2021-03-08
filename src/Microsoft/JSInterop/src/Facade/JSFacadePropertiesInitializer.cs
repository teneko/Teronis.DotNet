using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadePropertiesInitializer : IJSFacadePropertiesInitializer
    {
        private readonly IIndependentJSFacadePropertiesInitializer propertiesInitializer;
        private readonly IJSFacadeResolver referenceWrapperResolver;

        public JSFacadePropertiesInitializer(
            IIndependentJSFacadePropertiesInitializer propertiesInitializer,
            IJSFacadeResolver referenceWrapperResolver)
        {
            this.propertiesInitializer = propertiesInitializer ?? throw new ArgumentNullException(nameof(propertiesInitializer));
            this.referenceWrapperResolver = referenceWrapperResolver ?? throw new ArgumentNullException(nameof(referenceWrapperResolver));
        }

        public Task<JSFacadePropertiesInitialization> InitializeFacadePropertiesAsync(object component) =>
            propertiesInitializer.InitializeFacadePropertiesAsync(component, referenceWrapperResolver);
    }
}
