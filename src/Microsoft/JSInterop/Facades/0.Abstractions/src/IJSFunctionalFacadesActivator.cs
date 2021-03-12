using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFunctionalFacadesActivator
    {
        ValueTask<IJSComponentFacades> CreateFacadesAsync(object component, IJSFacadeResolver jsFacadeResolver);
        ValueTask<IJSComponentFacades> CreateFacadesAsync(IJSFacadeResolver jsFacadeResolver);
    }
}
