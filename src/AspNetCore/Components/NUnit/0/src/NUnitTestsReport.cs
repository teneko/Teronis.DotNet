using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Teronis.AspNetCore.Components.NUnit
{
    public class NUnitTestsReport : ComponentBase
    {
        [Parameter] public string XmlReportDivId { get; set; } = NUnitTestsReportDefaults.XML_REPORT_DIV_ID;
        [Parameter] public string? XmlReport { get; set; } = null!;

        private RenderTreeSequence sequence = new RenderTreeSequence();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            sequence.Reset();
            builder.OpenElement(sequence, "div");
            builder.AddAttribute(sequence, "id", XmlReportDivId);
            builder.AddContent(sequence, XmlReport);
            builder.CloseElement();
        }
    }
}
