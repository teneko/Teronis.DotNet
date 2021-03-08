using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facade.Annotiations.Design;

namespace Teronis_._Microsoft.JSInterop.Tests.JSFacades
{
    [JSModuleFacade("./js/es-modules/getTonyHawk.js")]
    public class GetTonyHawkModule : IAsyncDisposable
    {
        private readonly IJSObjectReference objectReference;

        public GetTonyHawkModule(IJSObjectReference objectReference) => 
            this.objectReference = objectReference ?? throw new ArgumentNullException(nameof(objectReference));

        public ValueTask<string> GetTonyHawkAsync() =>
            objectReference.InvokeAsync<string>("getTonyHawk");

        public ValueTask DisposeAsync() =>
            objectReference.DisposeAsync();
    }
}
