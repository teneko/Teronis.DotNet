// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;
using Teronis_._Microsoft.JSInterop.Modules;

namespace Teronis_._Microsoft.JSInterop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var services = builder.Services;

            services.AddScoped(serviceProvider => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            services.AddJSDynamicProxyActivator(configureInterceptorServicesOptions: options =>
                options.ConfigureInterceptorServices(builder =>
                    builder.UseExtension(e => e.AddScoped<JSDynamicProxyActivatingInterceptor>())));

            services.AddJSCustomFacadeActivator(options => {
                options.ConfigureCustomFacadeServices(services =>
                    services.UseExtension(extension =>
                        extension.AddScoped<TonyHawkModule>()));
            });

            services.AddJSDynamicFacadeHub();
            await builder.Build().RunAsync();
        }
    }
}
