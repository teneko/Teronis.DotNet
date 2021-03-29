// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using NUnit.Framework;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Facades.Annotations;
using Teronis.NUnit.TaskTests;
using Teronis_._Microsoft.JSInterop.Facades.JSModules;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    // Compare Program.cs.
    //[TaskTestCaseBlockStaticMemberProvider(nameof(Instance))]
    public class JSFacadesDependencyInjectedModuleTests : TaskTestCaseBlock
    {
        public readonly static JSFacadesDependencyInjectedModuleTests Instance = null!;

        public IJSFacadesActivator jsFacadesActivator { get; set; } = null!;

        public JSFacadesDependencyInjectedModuleTests(IJSFacadesActivator jsFacadesActivator) =>
            this.jsFacadesActivator = jsFacadesActivator ?? throw new ArgumentNullException(nameof(jsFacadesActivator));

        [JSModuleProperty] ModuleActivationViaDependencyInjection Module { get; set; } = null!;

        public TaskTestCase Should_resolve_module_via_dependency_injection = AddTest(async (_) => {
            var jsFacades = await Instance.jsFacadesActivator.CreateInstanceAsync<JSFacadeActivators>(Instance);
            var tonyHawk = await Instance.Module.GetTonyHawkAsync();

            // Assert
            Assert.AreEqual(ModuleActivationViaDependencyInjection.ExpectedTonyHawkContent, tonyHawk);
        });
    }
}
