// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadeHubActivator : IInstanceActivator<IJSFacadeHub<IJSFacadeActivators>>
    {
        /// <summary>
        /// Initializes the properties of the component that are decorated with a facade attribute.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        ValueTask<IJSFacadeHub<TJSFacadeActivators>> CreateInstanceAsync<TJSFacadeActivators>(object component)
            where TJSFacadeActivators : IJSFacadeActivators;

        /// <summary>
        /// Creates an empty container for facades.
        /// </summary>
        /// <returns></returns>
        IJSFacadeHub<TJSFacadeActivators> CreateInstance<TJSFacadeActivators>()
            where TJSFacadeActivators : IJSFacadeActivators;
    }
}
