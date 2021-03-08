using System;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Annotiations
{
    public abstract class JSModuleFacadeAttributeBase : Attribute
    {
        /// <summary>
        /// The path of a JavaScript ESM module relative to "wwwroot" (if not changed)
        /// </summary>
        public string? PathRelativeToWwwRoot { get; }

        /// <summary>
        /// The properties (owned by component - see 
        /// <see cref="JSFacadesInitializer.InitializeFacadesAsync(object)"/>) of type <see cref="IJSObjectReference"/>
        /// gets initialized when decorated with this attribute.
        /// </summary>
        public JSModuleFacadeAttributeBase() { }

        /// <summary>
        /// The properties (owned by component - see 
        /// <see cref="JSFacadesInitializer.InitializeFacadesAsync(object)"/>) of type <see cref="IJSObjectReference"/>
        /// gets initialized when decorated with this attribute.
        /// </summary>
        /// <param name="pathRelativeToWwwRoot">Relative path where the working directoy is typcially wwwroot.</param>
        public JSModuleFacadeAttributeBase(string pathRelativeToWwwRoot) =>
            PathRelativeToWwwRoot = pathRelativeToWwwRoot; 
    }
}
