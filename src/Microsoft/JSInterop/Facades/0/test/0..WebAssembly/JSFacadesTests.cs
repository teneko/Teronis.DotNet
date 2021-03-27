// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using NUnit.Framework;
using Teronis.Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Dynamic.Module;
using Teronis_._Microsoft.JSInterop.Facades.JSDynamicObjects;
using Microsoft.JSInterop;
using Teronis.NUnit.TaskTests;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    [TaskTestCaseBlockStaticMemberProvider(nameof(Instance))]
    public class JSFacadesTests : TaskTestCaseBlock<JSFacadesTests>
    {
        public readonly static JSFacadesTests Instance = null!;

        private IJSFacadesActivator jsFacadesActivator;

        public JSFacadesTests(IJSFacadesActivator serviceProvider) =>
            jsFacadesActivator = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public TaskTestCase Should_first_call_dynamic_invoke_and_then_call_inbuilt_invoke = AddTest(async (_) => {
            var jsFacades = Instance.jsFacadesActivator.CreateInstance<JSDynamicFacadeActivators>();
            var jsModule = await jsFacades.Activators.JsDynamicModuleActivator.CreateInstanceAsync<IMomentDynamicObject>("./js/esm-bundle.js");

            var moment = await jsModule.moment("2013-02-08 09");
            var formattedDate = await moment.InvokeAsync<string>("format");
            StringAssert.StartsWith("2013-02-08", formattedDate);
        });
    }
}
