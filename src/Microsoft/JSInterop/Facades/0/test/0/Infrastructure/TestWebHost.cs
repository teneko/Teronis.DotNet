// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Teronis.Microsoft.JSInterop.Facade.Infrastructure
{
    public class TestWebHost : TestHost
    {
        protected virtual void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder) { }

        protected override IHost CreateHost() =>
            new HostBuilder()
                .ConfigureWebHost(webHostBuilder => {
                    webHostBuilder
                        .UseKestrel()
                        .UseStaticWebAssets()
                        .UseUrls($"http://127.0.0.1:0"); // :0 allows to choose a port automatically

                    ConfigureWebHostBuilder(webHostBuilder);
                })
                .Build();
    }
}
