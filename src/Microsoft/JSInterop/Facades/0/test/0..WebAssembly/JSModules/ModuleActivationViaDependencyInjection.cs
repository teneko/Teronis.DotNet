using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facades.Annotations;

namespace Teronis_._Microsoft.JSInterop.Facades.JSModules
{
    [JSModuleClass("./js/es-modules/getTonyHawk.js")]
    public class ModuleActivationViaDependencyInjection : IAsyncDisposable
    {
        public const string ExpectedTonyHawkContent = "Tony Hawk";

        private readonly IJSObjectReferenceFacade jsObjectReferenceFacade;

        public ModuleActivationViaDependencyInjection(IJSObjectReferenceFacade jsObject) => 
            this.jsObjectReferenceFacade = jsObject ?? throw new ArgumentNullException(nameof(jsObject));

        public ValueTask<string> GetTonyHawkAsync() =>
            jsObjectReferenceFacade.InvokeAsync<string>("getTonyHawk");

        public ValueTask DisposeAsync() =>
            jsObjectReferenceFacade.DisposeAsync();
    }
}
