// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Activators;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Facade;
using Teronis.Microsoft.JSInterop.Module;
using Xunit;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFacadeHubComponentTests
    {
        [AssignModule("module")]
        [JSCustomFacade]
        private IJSModule module { get; set; } = null!;

        [Fact]
        public async Task Should_initalize_component()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IJSModuleActivator, EmptyJSModuleActivator>()
                .AddJSFacadeHub()
                .BuildServiceProvider();

            await using var facadeHub = await serviceProvider
                .GetRequiredService<IJSFacadeHubActivator>()
                .CreateInstanceAsync<JSFacadeActivators>(this);

            Assert.True(facadeHub.ComponentDisposables.Count == 1);
            Assert.Equal("module", module.ModuleNameOrPath);
        }
    }
}
