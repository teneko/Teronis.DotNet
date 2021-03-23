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
    public class JSFacadesDependencyInjectedModuleTests : TaskTests<JSFacadesTests>
    {
        public readonly static JSFacadesDependencyInjectedModuleTests Instance = null!;

        public IServiceProvider ServiceProvider { get; set; } = null!;

        [ActivatorUtilitiesConstructor]
        public JSFacadesDependencyInjectedModuleTests(IServiceProvider serviceProvider) =>
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        [JSModuleProperty] ModuleActivationViaDependencyInjection Module { get; set; } = null!;

        private T GetService<T>() =>
            ActivatorUtilities.GetServiceOrCreateInstance<T>(Instance.ServiceProvider);

        public LazyTask Should_resolve_module_via_dependency_injection = AddTest(async () => {
            var jsFacadesActivator = Instance.GetService<IJSFacadesActivator>();
            var jsFacades = await jsFacadesActivator.CreateInstanceAsync<JSFacadeActivators>(Instance);
            var tonyHawk = await Instance.Module.GetTonyHawkAsync();
            Assert.AreEqual(ModuleActivationViaDependencyInjection.ExpectedTonyHawkContent, tonyHawk);
        });
    }
}
