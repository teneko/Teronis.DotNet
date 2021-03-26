// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facades.Infrastructure;
using Xunit;
using System;
using PlaywrightSharp;
using Teronis.AspNetCore.Components.NUnit;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadeTests : IClassFixture<TestWebHostFixture<WebHostStartup>>, IClassFixture<PlaywrightFixture>
    {
        private readonly TestWebHostFixture<WebHostStartup> server;
        private readonly PlaywrightFixture playwright;

        public JSFacadeTests(TestWebHostFixture<WebHostStartup> server, PlaywrightFixture playwright)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.playwright = playwright ?? throw new ArgumentNullException(nameof(playwright));
        }

        [Fact]
        public async Task Should_have_empty_nunit_report() {
            var applicationUrl = server.ApplicationUrl;
            await using var browser = await playwright.Instance.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var applicationUri = new Uri(applicationUrl);
            var pageUri = new Uri(applicationUri, "/");
            await page.GoToAsync(pageUri.AbsoluteUri, LifecycleEvent.Networkidle);
            var nunitXmlReport = await page.GetTextContentAsync(NUnitTestsReportDefaults.XML_REPORT_DIV_ID_HASHED);
            Assert.Equal(string.Empty, nunitXmlReport);
        }
    }
}
