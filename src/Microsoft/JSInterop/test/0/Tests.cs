using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;
using System;
using PlaywrightSharp;

namespace Teronis.Microsoft.JSInterop.Test
{
    public class Tests : IClassFixture<TestWebHostFixture<WebHostStartup>>, IClassFixture<PlaywrightFixture>
    {
        private readonly ITestOutputHelper output;
        private readonly TestWebHostFixture<WebHostStartup> server;
        private readonly PlaywrightFixture playwright;

        public Tests(ITestOutputHelper output, TestWebHostFixture<WebHostStartup> server, PlaywrightFixture playwright)
        {
            this.output = output;
            this.server = server;
            this.playwright = playwright;
        }

        [Fact]
        public async Task Should()
        {
            var applicationUrl = server.ApplicationUrl;
            await using var browser = await playwright.Instance.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var applicationUri = new Uri(applicationUrl);
            var pageUri = new Uri(applicationUri, "get-tony-hawk");
            await page.GoToAsync(pageUri.AbsoluteUri, LifecycleEvent.Networkidle);
            var tonyHawkContent = await page.GetTextContentAsync("#tony-hawk");
            Assert.Contains("Tony Hawk", tonyHawkContent);
        }
    }
}
