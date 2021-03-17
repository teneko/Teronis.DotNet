using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Modules.Dynamic;
using Teronis_._Microsoft.JSInterop.Facades.JSModules;

namespace Teronis_._Microsoft.JSInterop.Facades
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var services = builder.Services;

            services.AddScoped(serviceProvider => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            services.AddJSDynamicProxies(); // temporary
            services.AddJSDynamicModules(); // temporary
            services.AddJSFacades();

            services.AddJSFacadeDictionary(builder => builder
                .AddFacade(objectReference => new UserCreatedModule(objectReference))
                .AddFacade<ServiceProviderCreatedModule>());

            await builder.Build().RunAsync();
        }
    }
}
