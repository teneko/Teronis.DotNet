using System;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facades.Annotiations
{
    public abstract class JSModuleFacadeAttributeBase : Attribute
    {
        /// <summary>
        /// The path of a JavaScript ESM module relative to "wwwroot" (if not changed)
        /// </summary>
        public string? ModuleNameOrPath { get; }

        /// <summary>
        /// The properties (owned by component - see 
        /// <see cref="JSFacadesActivator.CreateInstanceAsync(object)"/>) of type <see cref="IJSObjectReference"/>
        /// gets initialized when decorated with this attribute.
        /// </summary>
        public JSModuleFacadeAttributeBase() { }

        /// <summary>
        /// The properties (owned by component - see 
        /// <see cref="JSFacadesActivator.CreateInstanceAsync(object)"/>) of type <see cref="IJSObjectReference"/>
        /// gets initialized when decorated with this attribute.
        /// </summary>
        /// <param name="moduleNameOrPath">Relative path where the working directoy is typcially wwwroot.</param>
        public JSModuleFacadeAttributeBase(string moduleNameOrPath) =>
            ModuleNameOrPath = moduleNameOrPath; 
    }
}
