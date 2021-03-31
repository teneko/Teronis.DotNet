// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facades.Infrastructure;
using Xunit;
using System;
using Teronis.AspNetCore.Components.NUnit;
using System.Xml.Linq;
using System.Linq;
using Xunit.Abstractions;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadeTests : IClassFixture<TestWebHostFixture<WebHostStartup>>, IClassFixture<PlaywrightFixture>
    {
        private readonly TestWebHostFixture<WebHostStartup> server;
        private readonly PlaywrightFixture playwright;
        private readonly ITestOutputHelper output;

        public JSFacadeTests(TestWebHostFixture<WebHostStartup> server, PlaywrightFixture playwright, ITestOutputHelper output)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.playwright = playwright ?? throw new ArgumentNullException(nameof(playwright));
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        [Fact]
        public async Task Should_have_passed_nunit_report()
        {
            var applicationUrl = server.ApplicationUrl;
            await using var browser = await playwright.Instance.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var applicationUri = new Uri(applicationUrl);
            var pageUri = new Uri(applicationUri, "/");
            await page.GoToAsync(pageUri.AbsoluteUri);
            await page.WaitForSelectorAsync(NUnitTestsReportDefaults.NUnitXmlReportRenderedAttributeSelector);
            var nunitXmlReport = await page.GetTextContentAsync(NUnitTestsReportDefaults.NUnitXmlReportIdSelector);
            output.WriteLine(nunitXmlReport);

            Assert.True(
                XDocument.Parse(nunitXmlReport)
                    .Descendants("test-suite")
                    .All(x => {
                        var attribute = x.Attribute("result");

                        if (attribute is null) {
                            throw new ArgumentNullException(nameof(attribute));
                        }

                        return attribute.Value.ToLower() == "passed";
                    }));
        }
    }
}
