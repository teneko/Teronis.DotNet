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
        [Inject] private IServiceProvider serviceProvider { get; set; } = null!;

        /// <summary>
        /// You must set either <see cref="Assembly"/> or <see cref="AssemblyType"/>.
        /// Parameter <see cref="Assembly"/> has precedence.
        /// </summary>
        [Parameter] public Assembly? Assembly { get; set; } = null!;
        /// <summary>
        /// You must set either <see cref="Assembly"/> or <see cref="AssemblyType"/>.
        /// Parameter <see cref="Assembly"/> has precedence.
        /// </summary>
        [Parameter] public Type? AssemblyType { get; set; } = null!;
        [Parameter] public EventCallback<IServiceProvider> BeforeAssertingTasks { get; set; }
        [Parameter] public ITestListener? TestListener { get; set; }
        [Parameter] public ITestFilter? TestFilter { get; set; }
        [Parameter] public string XmlReportDivId { get; set; } = NUnitTestsReportDefaults.XML_REPORT_DIV_ID;

        private string? xmlReport { get; set; }
        private RenderTreeSequence sequence = new RenderTreeSequence();

        private async Task RunTestsAsync()
        {
            var assembly = Assembly
                ?? AssemblyType?.Assembly
                ?? throw new ArgumentException($"You must set either {nameof(Assembly)} or {nameof(AssemblyType)}.");

            if (BeforeAssertingTasks.HasDelegate) {
                await BeforeAssertingTasks.InvokeAsync(serviceProvider);
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
                xmlReport = result.GenerateXmlReport();
            } catch (Exception e) {
                xmlReport = e.ToString();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            /* By calling it in OnAfterRenderAsync we should support Blazor Server. */

            if (firstRender) {
                await RunTestsAsync();
                // Is now supported in OnAfterRenderAsync cycle.
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            sequence.Reset();
            builder.OpenComponent<NUnitTestsReport>(sequence);
            builder.AddAttribute(sequence, nameof(NUnitTestsReport.XmlReportDivId), XmlReportDivId);
            builder.AddAttribute(sequence, nameof(NUnitTestsReport.XmlReport), xmlReport);
            builder.CloseComponent();
        }
    }
}
