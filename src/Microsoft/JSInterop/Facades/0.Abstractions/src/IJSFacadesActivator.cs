using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadesActivator
    {
        /// <summary>
        /// Initializes the properties of the component that are decorated with a facade attribute.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        ValueTask<IJSFacades> CreateInstanceAsync(object component);
        /// <summary>
        /// Creates an empty container for facades.
        /// </summary>
        /// <returns></returns>
        IJSFacades CreateInstance();
    }
}
