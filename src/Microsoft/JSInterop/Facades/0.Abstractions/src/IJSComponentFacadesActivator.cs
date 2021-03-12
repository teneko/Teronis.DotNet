using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSComponentFacadesActivator
    {
        /// <summary>
        /// Initializes the properties of the component that are decorated with a facade attribute.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        ValueTask<IJSComponentFacades> CreateInstance(object component);
        /// <summary>
        /// Creates an empty container for facades.
        /// </summary>
        /// <returns></returns>
        IJSComponentFacades CreateEmptyInstance();
    }
}
