using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Teronis.Mvc.Hosting;

[assembly: HostingStartup(typeof(Teronis.Identity.IdentityHostingStartup))]

namespace Teronis.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                services.PostConfigureMvcBuilderContext(context => {
                    var fullAssemblyName = typeof(IdentityHostingStartup).Assembly.GetName().Name;
                    var partManager = context.PartManager.ApplicationParts.FirstOrDefault(x => x.Name == fullAssemblyName);

                    if (!(partManager is null)) {
                        context.PartManager.ApplicationParts.Remove(partManager);
                    }
                });
            });
        }
    }
}
