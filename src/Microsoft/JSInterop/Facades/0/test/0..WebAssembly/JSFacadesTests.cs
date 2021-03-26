// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Teronis.Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Dynamic.Module;
using Teronis_._Microsoft.JSInterop.Facades.JSDynamicObjects;
using Microsoft.JSInterop;
using Teronis.NUnit.TaskTests;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    [TaskTests(nameof(Instance))]
    public class JSFacadesTests : TaskTests<JSFacadesTests>
    {
        public readonly static JSFacadesTests Instance = null!;

        public IServiceProvider ServiceProvider { get; set; } = null!;

        public JSFacadesTests(LazyTaskList? testTasks)
            : base(testTasks) { }

        public JSFacadesTests()
            : this(testTasks: null) { }

        [ActivatorUtilitiesConstructor]
        public JSFacadesTests(IServiceProvider serviceProvider) =>
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        private T GetService<T>() =>
            ActivatorUtilities.GetServiceOrCreateInstance<T>(Instance.ServiceProvider);

        public LazyTask Should_first_call_dynamic_invoke_and_then_call_inbuilt_invoke = AddTest(async () => {
            var jsFacadesActivator = Instance.GetService<IJSFacadesActivator>();
            var jsFacades = jsFacadesActivator.CreateInstance<JSDynamicFacadeActivators>();
            var jsModule = await jsFacades.Activators.JsDynamicModuleActivator.CreateInstanceAsync<IMomentDynamicObject>("./js/esm-bundle.js");

            var moment = await jsModule.moment("2013-02-08 09");
            var formattedDate = await moment.InvokeAsync<string>("format");
            StringAssert.StartsWith("2013-02-08", formattedDate);
        });
    }
}
