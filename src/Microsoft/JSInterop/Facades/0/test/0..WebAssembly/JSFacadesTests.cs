// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using NUnit.Framework;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis_._Microsoft.JSInterop.Facades.JSDynamicObjects;
using Teronis.NUnit.TaskTests;
using Teronis.Microsoft.JSInterop.Dynamic.Activators;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    [TaskTestCaseBlockStaticMemberProvider(nameof(Instance))]
    public class JSFacadesTests : TaskTestCaseBlock
    {
        public readonly static JSFacadesTests Instance = null!;

        private IJSFacadesActivator jsFacadesActivator;

        public JSFacadesTests(IJSFacadesActivator serviceProvider) =>
            jsFacadesActivator = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public TaskTestCase Should_first_call_dynamic_invoke_and_then_call_inbuilt_invoke = AddTest(async (_) => {
            var jsFacades = Instance.jsFacadesActivator.CreateInstance<JSDynamicFacadeActivators>();
            var jsModule = await jsFacades.Activators.JSDynamicModuleActivator.CreateInstanceAsync<IMomentDynamicModule>("./js/esm-bundle.js");
            await using var moment = await jsModule.moment("2013-02-08 09");
            var formattedDate = await moment.format();
            StringAssert.StartsWith("2013-02-08", formattedDate);
        });
    }
}
