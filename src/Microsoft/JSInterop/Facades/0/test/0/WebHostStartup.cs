// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class WebHostStartup
    {
        public void ConfigureServices(IServiceCollection services) =>
            services.AddRouting();

        public void Configure(IApplicationBuilder app)
        {
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapFallbackToFile(
                    "/{*path:nonfile}",
                    "/index.html");
            });
        }
    }
}
