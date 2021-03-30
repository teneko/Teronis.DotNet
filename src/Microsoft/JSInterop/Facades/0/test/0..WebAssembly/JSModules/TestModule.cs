// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;

namespace Teronis_._Microsoft.JSInterop.Facades.JSModules
{
    [JSModuleClass("./js/Teronis.Microsoft.JSInterop.Facades.E2ETest.WebAssembly.js")]
    public class TestModule : IAsyncDisposable
    {
        private readonly IJSObjectReference objectReference;

        public TestModule(IJSObjectReference objectReference) => 
            this.objectReference = objectReference ?? throw new ArgumentNullException(nameof(objectReference));

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
