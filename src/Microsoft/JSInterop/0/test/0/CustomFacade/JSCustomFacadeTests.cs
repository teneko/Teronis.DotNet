// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Activators;
using Teronis.Microsoft.JSInterop.Modules;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.ObjectReferences;
using Xunit;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public class JSCustomFacadeTests
    {
        [Theory]
        [MemberData(nameof(BuildCustomFacdeActivators))]
        public void Should_activate_custom_module(IJSCustomFacadeActivator jsCustomFacadeActivator)
        {
            var customModule = jsCustomFacadeActivator.CreateInstance<CustomModule>(new JSModule(new EmptyObjectReference(), "module", jsInterceptor: null));

            // Assert
            Assert.Equal("module", customModule.Module.ModuleNameOrPath);
        }

        public static IServiceCollection CreateServiceCollection() =>
            new ServiceCollection()
                .AddSingleton<IJSModuleActivator, EmptyModuleActivator>()
                .AddJSCustomFacade();

        public static IEnumerable<object[]> BuildCustomFacdeActivators()
        {
            yield return CreateServiceCollection()
                .AddJSCustomFacadeActivator(configureOptions: options =>
                    options.JSFacadeDictionaryBuilder
                        .Add(typeof(CustomModule), serviceProvider =>
                            new CustomModule(serviceProvider.GetRequiredService<IJSModule>())))
                .BuildServiceProvider()
                .GetRequiredService<IJSCustomFacadeActivator>()
                .AsArray();

            yield return CreateServiceCollection()
                .AddJSCustomFacadeActivator(configureOptions: options =>
                    options.JSFacadeDictionaryBuilder
                        .Add<CustomModule>())
                .BuildServiceProvider()
                .GetRequiredService<IJSCustomFacadeActivator>()
                .AsArray();
        }
    }
}
