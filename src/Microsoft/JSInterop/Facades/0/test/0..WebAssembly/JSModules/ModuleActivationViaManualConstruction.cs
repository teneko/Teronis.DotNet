using Teronis.Microsoft.JSInterop;

namespace Teronis_._Microsoft.JSInterop.Facades.JSModules
{
    // JavaScript module path is inherited from base class.
    public class ModuleActivationViaManualConstruction : ModuleActivationViaDependencyInjection
    {
        public ModuleActivationViaManualConstruction(IJSObjectReferenceFacade jsObjectReference)
            : base(jsObjectReference) { }
    }
}
