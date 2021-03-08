using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSIndependentFacadesInitializer
    {
        Task<JSFacades> InitializeFacadesAsync(object component, IJSFacadeResolver jsFacadeResolver);
        Task<JSFacades> InitializeFacadesAsync(IJSFacadeResolver jsFacadeResolver);
    }
}
