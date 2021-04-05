// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Modules;
using Teronis.Microsoft.JSInterop.ObjectReferences;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSDynamicFacadeHubComponentTests
    {
        [AssignDynamicModule(NameOrPath = "module")]
        private IJSModule module { get; set; } = null!;

        [AssignDynamicModule(NameOrPath = "module", InterfaceToBeProxied = typeof(IJSModule)), AssignCustomFacade]
        CustomModule customModule = null!;

        [AssignDynamicGlobalObject]
        IJSLocalObject window { get; set; } = null!;

        [Fact]
        public async Task Should_initalize_component()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IJSModuleActivator, EmptyModuleActivator>()
                .AddSingleton<IJSLocalObjectActivator, EmptyLocalObjectActivator>()
                .AddJSCustomFacadeActivator(configureOptions: options =>
                    options.ConfigureCustomFacadeServices(services =>
                        services.UseExtension(extension =>
                            extension.AddScoped<CustomModule>())))
                .AddJSDynamicFacadeHub()
                .BuildServiceProvider();

            await using var facadeHub = await serviceProvider
                .GetRequiredService<IJSFacadeHubActivator>()
                .CreateInstanceAsync<JSFacadeActivators>(this);

            Assert.Equal("module", module.ModuleNameOrPath);
            Assert.Equal("module", customModule.Module.ModuleNameOrPath);

            var typedWindow = Assert.IsType<EmptyObjectReference>(window.ObjectReference);
            Assert.Equal(nameof(EmptyLocalObjectActivator), typedWindow.Tag);
        }
    }
}
