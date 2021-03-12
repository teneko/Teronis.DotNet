using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFunctionalFacadesActivator
    {
        ValueTask<IJSComponentFacades> CreateInstanceAsync(object component, IJSFacadeResolver jsFacadeResolver);
        IJSComponentFacades CreateEmptyInstance(IJSFacadeResolver jsFacadeResolver);
    }
}
