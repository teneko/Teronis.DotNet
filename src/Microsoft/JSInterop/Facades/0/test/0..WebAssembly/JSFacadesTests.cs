// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using NUnit.Framework;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis_._Microsoft.JSInterop.Facades.JSDynamicObjects;
using Teronis.NUnit.TaskTests;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    [TaskTestCaseBlockStaticMemberProvider(nameof(Instance))]
    public class JSFacadesTests : TaskTestCaseBlock
    {
        public readonly static JSFacadesTests Instance = null!;

        private readonly IJSFacadeHub<JSDynamicFacadeActivators> jsFacadeHub;

        public JSFacadesTests(IJSFacadeHub<JSDynamicFacadeActivators> jsFacadeHub) =>
            this.jsFacadeHub = jsFacadeHub ?? throw new ArgumentNullException(nameof(jsFacadeHub));

        public TaskTestCase Should_first_call_dynamic_invoke_and_then_call_inbuilt_invoke = AddTest(async (_) => {
            var jsFacadeHub = Instance.jsFacadeHub;
            var jsModule = await jsFacadeHub.Activators.JSDynamicModuleActivator.CreateInstanceAsync<IMomentDynamicModule>("./js/esm-bundle.js");
            await using var moment = await jsModule.moment("2013-02-08 09");
            var formattedDate = await moment.format();
            StringAssert.StartsWith("2013-02-08", formattedDate);
        });
    }
}
