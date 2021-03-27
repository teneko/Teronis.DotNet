// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NUnit.Framework.Interfaces;
using Teronis.NUnit.Api;

namespace Teronis.AspNetCore.Components.NUnit
{
    public class NUnitTests : ComponentBase
    {
        /// <summary>
        /// The assembly where test fixtures are going to be searched.
        /// You must set either <see cref="TestExplorationAssembly"/> or <see cref="TestExplorationAssemblyType"/>.
        /// Parameter <see cref="TestExplorationAssembly"/> has precedence.
        /// </summary>
        [Parameter] public Assembly? TestExplorationAssembly { get; set; } = null!;
        /// <summary>
        /// The assembly of specified type where test fixtures are going to be searched.
        /// You must set either <see cref="TestExplorationAssembly"/> or <see cref="TestExplorationAssemblyType"/>.
        /// Parameter <see cref="TestExplorationAssembly"/> has precedence.
        /// </summary>
        [Parameter] public Type? TestExplorationAssemblyType { get; set; } = null!;
        [Parameter] public EventCallback<IServiceProvider> BeforeRunningTests { get; set; }
        [Parameter] public ITestListener? TestListener { get; set; }
        [Parameter] public ITestFilter? TestFilter { get; set; }
        [Parameter] public string XmlReportDivId { get; set; } = NUnitTestsReportDefaults.NUnitXmlReportIdName;
        [Parameter] public bool IdentXmlReport { get; set; }
        /// <summary>
        /// The attribute gets passed through to <see cref="NUnitTestsReport"/> after the test XML report has been
        /// calculated.
        /// </summary>
        [Parameter] public string? XmlReportRenderedAttributeName { get; set; } = NUnitTestsReportDefaults.NUnitXmlReportRenderedAttributeName;


        [Inject] internal protected IServiceProvider ServiceProvider { get; set; } = null!;
        private string? xmlReport { get; set; }
        private bool xmlReportRendered;

        private async Task RunTestsAsync()
        {
            var assembly = TestExplorationAssembly
                ?? TestExplorationAssemblyType?.Assembly
                ?? throw new ArgumentException($"You must set either {nameof(TestExplorationAssembly)} or {nameof(TestExplorationAssemblyType)}.");

            if (BeforeRunningTests.HasDelegate) {
                await BeforeRunningTests.InvokeAsync(ServiceProvider);
            }

            try {
                var runner = new NUnitSingleThreadAssemblyRunner();
                runner.LoadTestsInAssembly(assembly);

                var result = runner.RunTests(
                    listener: TestListener,
                    filter: TestFilter);

#if DEBUG
                Console.WriteLine(result.GenerateSummaryReport());
#endif
                xmlReport = result.GenerateXmlReport(ident: IdentXmlReport);
            } catch (Exception e) {
                xmlReport = e.ToString();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            /* By calling it in OnAfterRenderAsync we should support Blazor Server. */

            if (firstRender) {
                await RunTestsAsync();
                xmlReportRendered = true;
                // Is now supported in OnAfterRenderAsync cycle.
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = new RenderTreeSequence();
            builder.OpenComponent<NUnitTestsReport>(sequence.Increment());
            builder.AddAttribute(sequence.Increment(), nameof(NUnitTestsReport.XmlReportDivId), XmlReportDivId);
            builder.AddAttribute(sequence.Increment(), nameof(NUnitTestsReport.XmlReport), xmlReport);

            var xmlReportRenderedAttributeName = xmlReportRendered
                ? XmlReportRenderedAttributeName
                : null;

            builder.AddAttribute(sequence.Increment(), nameof(NUnitTestsReport.XmlReportRenderedAttributeName), xmlReportRenderedAttributeName);
            builder.CloseComponent();
        }
    }
}
