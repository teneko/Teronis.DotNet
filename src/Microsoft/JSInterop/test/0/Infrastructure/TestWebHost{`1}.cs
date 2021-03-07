using Microsoft.AspNetCore.Hosting;

namespace Teronis.Microsoft.JSInterop.Test.Infrastructure
{
    public class TestWebHost<StartupClass> : TestWebHost
        where StartupClass : class
    {
        protected override void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder) {
            base.ConfigureWebHostBuilder(webHostBuilder);
            webHostBuilder.UseStartup<StartupClass>();
        }
    }
}
