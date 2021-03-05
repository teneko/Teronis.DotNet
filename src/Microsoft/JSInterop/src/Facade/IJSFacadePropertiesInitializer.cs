using System.Threading.Tasks;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public interface IJSFacadePropertiesInitializer
    {
        /// <summary>
        /// Initializes the properties of the component that are decorated with a facade attribute.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        Task<JSFacadePropertiesInitialization> InitializeFacadePropertiesAsync(object component);
    }
}
