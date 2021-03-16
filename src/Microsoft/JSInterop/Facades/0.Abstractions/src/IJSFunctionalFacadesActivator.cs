using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFunctionalFacadesActivator
    {
        ValueTask<IJSFacades> CreateInstanceAsync(object component, IJSFacadeResolver jsFacadeResolver);
        IJSFacades CreateEmptyInstance(IJSFacadeResolver jsFacadeResolver);
    }
}
