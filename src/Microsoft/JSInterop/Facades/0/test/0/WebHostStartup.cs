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
