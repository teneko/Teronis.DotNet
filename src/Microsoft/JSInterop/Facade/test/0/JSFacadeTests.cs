using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Facade.Infrastructure;
using Xunit;
using System;
using PlaywrightSharp;
using Teronis_._Microsoft.JSInterop.Facade;

namespace Teronis.Microsoft.JSInterop.Facade
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

        private async Task TestPageRegardingModuleResolution(string relativePath) {
            var applicationUrl = server.ApplicationUrl;
            await using var browser = await playwright.Instance.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            var applicationUri = new Uri(applicationUrl);
            var pageUri = new Uri(applicationUri, relativePath);
            await page.GoToAsync(pageUri.AbsoluteUri, LifecycleEvent.Networkidle);
            var tonyHawkContent = await page.GetTextContentAsync(ShoudlyPagesDefaults.TonyHawkIdSelector);
            Assert.Contains(ShoudlyPagesDefaults.ExpectedTonyHawkContent, tonyHawkContent);
        }

        [Fact]
        public Task Should_resolve_service_provider_created_module() =>
            TestPageRegardingModuleResolution(ShoudlyPages.should_resolve_service_provider_created_module);

        [Fact]
        public Task Should_resolve_user_created_module() =>
            TestPageRegardingModuleResolution(ShoudlyPages.should_resolve_user_created_module);
    }
}
