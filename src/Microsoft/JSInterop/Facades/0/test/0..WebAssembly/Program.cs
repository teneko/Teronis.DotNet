using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Locality.Dynamic;
using Teronis.Microsoft.JSInterop.Module.Dynamic;
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

            services.AddJSDynamicModule(); // temporary
            services.AddJSDynamicLocalObject();

            services.AddJSFacadeResolver(options => {
                options.JSFacadeDictionaryConfiguration
                    .Add(jsObjectReference => new UserCreatedModule(jsObjectReference))
                    .Add<ServiceProviderCreatedModule>();
            });

            services.AddJSFacades();

            await builder.Build().RunAsync();
        }
    }
}
