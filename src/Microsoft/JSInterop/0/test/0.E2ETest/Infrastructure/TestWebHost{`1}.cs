// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;

namespace Teronis.Microsoft.JSInterop.Infrastructure
{
    public class TestWebHost<StartupClass> : TestWebHost
        where StartupClass : class
    {
        protected override void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder)
        {
            base.ConfigureWebHostBuilder(webHostBuilder);
            webHostBuilder.UseStartup<StartupClass>();
        }
    }
}
