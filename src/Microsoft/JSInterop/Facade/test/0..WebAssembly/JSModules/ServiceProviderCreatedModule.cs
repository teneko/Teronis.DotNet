using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facade;
using Teronis.Microsoft.JSInterop.Facade.Annotiations.Design;

namespace Teronis_._Microsoft.JSInterop.Facade.JSModules
{
    [JSModuleFacade("./js/es-modules/getTonyHawk.js")]
    public class ServiceProviderCreatedModule : IAsyncDisposable
    {
        private readonly IJSLocalObject jsObject;

        public ServiceProviderCreatedModule(IJSLocalObject jsObject) => 
            this.jsObject = jsObject ?? throw new ArgumentNullException(nameof(jsObject));

        public ValueTask<string> GetTonyHawkAsync() =>
            jsObject.JSObjectReference.InvokeAsync<string>("getTonyHawk");

        public ValueTask DisposeAsync() =>
            jsObject.DisposeAsync();
    }
}
