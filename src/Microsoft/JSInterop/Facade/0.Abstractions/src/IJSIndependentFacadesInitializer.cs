using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSIndependentFacadesInitializer
    {
        ValueTask<IJSFacades> InitializeFacadesAsync(object component, IJSFacadeResolver jsFacadeResolver);
        ValueTask<IJSFacades> InitializeFacadesAsync(IJSFacadeResolver jsFacadeResolver);
    }
}
