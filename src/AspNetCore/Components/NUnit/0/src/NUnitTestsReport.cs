// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Teronis.AspNetCore.Components.NUnit
{
    public class NUnitTestsReport : ComponentBase
    {
        [Parameter] public string XmlReportDivId { get; set; } = NUnitTestsReportDefaults.NUnitXmlReportIdName;
        [Parameter] public string? XmlReport { get; set; } = null!;
        [Parameter] public string? XmlReportRenderedAttributeName { get; set; } = NUnitTestsReportDefaults.NUnitXmlReportRenderedAttributeName;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = new RenderTreeSequence();
            builder.OpenElement(sequence.Increment(), "pre");
            builder.AddAttribute(sequence.Increment(), "lang", "xml");
            builder.AddAttribute(sequence.Increment(), "id", XmlReportDivId);

            var subSequence = sequence.PlanIncrement();
            if (XmlReportRenderedAttributeName != null) {
                builder.AddAttribute(subSequence.Increment(), XmlReportRenderedAttributeName);
            }

            builder.AddContent(sequence.Increment(), XmlReport);
            builder.CloseElement();
        }
    }
}
