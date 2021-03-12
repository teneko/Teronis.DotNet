using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facade.Annotiations.Design;

namespace Teronis_._Microsoft.JSInterop.Facade.JSModules
{
    [JSModuleFacade("./js/Teronis.Microsoft.JSInterop.Facade.Test.WebAssembly.js")]
    public class TestModule : IAsyncDisposable
    {
        private readonly IJSObjectReference objectReference;

        public TestModule(IJSObjectReference objectReference) => 
            this.objectReference = objectReference ?? throw new System.ArgumentNullException(nameof(objectReference));

        public ValueTask<string> getCurrentTime()
        {
            return objectReference.InvokeAsync<string>("getCurrentTime");
        }

        public async ValueTask<string> getCurrentTime2()
        {
            var test = await objectReference.InvokeAsync<IJSObjectReference>("moment");
            return await test.InvokeAsync<string>("format");
        }

        public ValueTask DisposeAsync() =>
            objectReference.DisposeAsync();
    }
}
