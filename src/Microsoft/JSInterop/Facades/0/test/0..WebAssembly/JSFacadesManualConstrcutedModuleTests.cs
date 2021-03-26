// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Facades.Annotations;
using Teronis.NUnit.TaskTests;
using Teronis_._Microsoft.JSInterop.Facades.JSModules;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    [TaskTests(nameof(Instance))]
    public class JSFacadesManualConstrcutedModuleTests : TaskTests<JSFacadesTests>
    {
        public readonly static JSFacadesManualConstrcutedModuleTests Instance = null!;

        public IServiceProvider ServiceProvider { get; set; } = null!;

        [ActivatorUtilitiesConstructor]
        public JSFacadesManualConstrcutedModuleTests(IServiceProvider serviceProvider) =>
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        [JSModuleProperty] ModuleActivationViaManualConstruction Module { get; set; } = null!;

        private T GetService<T>() =>
            ActivatorUtilities.GetServiceOrCreateInstance<T>(Instance.ServiceProvider);

        public LazyTask Should_resolve_module_via_manual_construction = AddTest(async () => {
            var jsFacadesActivator = Instance.GetService<IJSFacadesActivator>();
            var jsFacades = await jsFacadesActivator.CreateInstanceAsync<JSFacadeActivators>(Instance);
            var tonyHawk = await Instance.Module.GetTonyHawkAsync();
#pragma warning disable IDE0002
            Assert.AreEqual(ModuleActivationViaManualConstruction.ExpectedTonyHawkContent, tonyHawk);
#pragma warning restore
        });
    }
}
