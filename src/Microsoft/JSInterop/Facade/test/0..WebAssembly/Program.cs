using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Facade;
using Teronis_._Microsoft.JSInterop.Facade.JSFacades;

namespace Teronis_._Microsoft.JSInterop.Facade
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var services = builder.Services;

            services.AddScoped(serviceProvider => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            services.AddJSFacade();

            services.AddJSFacadeDictionary(builder => builder
                .AddFacade(objectReference => new UserCreatedModule(objectReference))
                .AddFacade<ServiceProviderCreatedModule>());

            await builder.Build().RunAsync();
        }
    }
}
