using System;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facades.Annotations
{
    public abstract class JSModuleAttributeBase : Attribute
    {
        /// <summary>
        /// The path of a JavaScript ESM module relative to "wwwroot" (if not changed).
        /// </summary>
        public string? ModuleNameOrPath { get; }

        /// <summary>
        /// The properties (owned by component - see 
        /// <see cref="JSFacadesActivator.CreateInstanceAsync(object)"/>) of type <see cref="IJSObjectReference"/>
        /// gets initialized when decorated with this attribute.
        /// </summary>
        public JSModuleAttributeBase() { }

        /// <summary>
        /// The properties (owned by component - see 
        /// <see cref="JSFacadesActivator.CreateInstanceAsync(object)"/>) of type <see cref="IJSObjectReference"/>
        /// gets initialized when decorated with this attribute.
        /// </summary>
        /// <param name="moduleNameOrPath">Relative path where the working directoy is typcially wwwroot.</param>
        public JSModuleAttributeBase(string moduleNameOrPath) =>
            ModuleNameOrPath = moduleNameOrPath;
    }
}
