// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.AspNetCore.Components.NUnit
{
    public class NUnitTestsReportDefaults
    {
        public const string NUnitXmlReportIdName = "nunit-xml-report";
        public const string NUnitXmlReportIdSelector = "#" + NUnitXmlReportIdName;

        public const string NUnitXmlReportRenderedAttributeName = "nunit-test-report-rendered";
        public const string NUnitXmlReportRenderedAttributeSelector = "[" + NUnitXmlReportRenderedAttributeName + "]";
    }
}
