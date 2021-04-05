# Teronis.AspNetCore.Components.NUnit [![Nuget](https://img.shields.io/nuget/v/Teronis.AspNetCore.Components.NUnit)][NuGet Package]

_Create and execute NUnit test cases in Blazor programmatically and display NUnit XML report_

:running: [Quick Start Guide](#quick-start-guide) &nbsp; | &nbsp; :package: [NuGet Package][NuGet Package]

## About

This project is a wrapper around [Teronis.NUnit.Core](/src/NUnit/Core). It allows you to execute test cases and display their results in form of a XML report with the help of a component.

## Installation

Package Managaer

```
Install-Package Teronis.AspNetCore.Components.NUnit -Version <type here version>
```

.NET CLI

```
dotnet add package Teronis.AspNetCore.Components.NUnit --version <type here version>
```

## Quick Start Guide

1\. Define a test case block:

```
using NUnit.Framework;
using Teronis.NUnit.TaskTests;
...

[TaskTestCaseBlockStaticMemberProvider(nameof(Instance))]
public class MomentTests : TaskTestCaseBlock
{
    public readonly static MomentTests Instance = null!;

    private readonly IJSFacadeHub<JSDynamicFacadeActivators> jsFacadeHub;

    public MomentTests(IJSFacadeHub<JSDynamicFacadeActivators> jsFacadeHub) =>
        this.jsFacadeHub = jsFacadeHub ?? throw new ArgumentNullException(nameof(jsFacadeHub));

    public TaskTestCase Should_first_call_dynamic_invoke_and_then_call_inbuilt_invoke = AddTest(async (_) => {
        var jsFacadeHub = Instance.jsFacadeHub;
        var jsModule = await jsFacadeHub.Activators.JSDynamicModuleActivator.CreateInstanceAsync<IMomentDynamicModule>("./js/esm-bundle.js");
        await using var moment = await jsModule.moment("2013-02-08 09");
        var formattedDate = await moment.format();
        StringAssert.StartsWith("2013-02-08", formattedDate);
    });
}
```

2\. Define a page component:

```
@using Teronis.AspNetCore.Components.NUnit
@using Teronis.NUnit.TaskTests
@page "/"

<NUnitTests TestExplorationAssemblyType="typeof(TaskTestCaseSourceTestFixture)" BeforeRunningTests="BeforeRunningTasks" IdentXmlReport="true" />

@code {
    private async Task BeforeRunningTasks(IServiceProvider serviceProvider)
    {
        // When debugging this amount of time is needed to catch up the debugger.
#if DEBUG
        await Task.Delay(3000);
#endif

        TaskTestCaseSourceTestFixture.TaskTestCases = await
            new AssemblyTaskTestCaseBlockStaticMemberCollector(typeof(Program).Assembly)
                .CollectMembers()
                .AssignInstances(new TaskTestCaseBlockMemberAssigner(), serviceProvider)
                .PrepareTasksAssertion(new TaskTestCaseBlockPreparer())
            .ToListAsync();
    }
}
```

3\. Start the WebAssembly server and go to `/`.

## Example

Please visit one of my E2E test projects in [src/Microsoft/JSInterop/0/test/0.E2ETest.WebAssembly](/src/Microsoft/JSInterop/0/test/0.E2ETest.WebAssembly).

[NuGet Package]: https://www.nuget.org/packages/Teronis.AspNetCore.Components.NUnit