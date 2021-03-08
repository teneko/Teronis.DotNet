using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IIndependentJSFacadePropertiesInitializer
    {
        Task<JSFacadePropertiesInitialization> InitializeFacadePropertiesAsync(object component, IJSFacadeResolver jsFacadeResolver);
    }
}
