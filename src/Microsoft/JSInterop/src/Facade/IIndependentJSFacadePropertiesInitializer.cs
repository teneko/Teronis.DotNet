using System.Threading.Tasks;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public interface IIndependentJSFacadePropertiesInitializer
    {
        Task<JSFacadePropertiesInitialization> InitializeFacadePropertiesAsync(object component, IJSFacadeResolver jsFacadeResolver);
    }
}
