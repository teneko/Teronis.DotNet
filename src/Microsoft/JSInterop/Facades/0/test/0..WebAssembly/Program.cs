// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Dynamic.Interceptors;
using Teronis.Microsoft.JSInterop.Dynamic.Locality;
using Teronis.Microsoft.JSInterop.Dynamic.Module;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Module;
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

            services.AddJSDynamicProxyActivator(options => {
                options.ConfigureInterceptorWalkerBuilder(builder =>
                    builder.AddInterceptor(typeof(JSDynamicProxyActivatingInterceptor)));
            });

            services.AddJSDynamicModule(); // temporary
            services.AddJSDynamicLocalObject();

            services.AddJSCustomFacadeActivator(options => {
                options.JSFacadeDictionaryConfiguration
                    .Add(serviceProvider => new ModuleActivationViaManualConstruction(serviceProvider.GetRequiredService<IJSModule>()))
                    .Add<ModuleActivationViaDependencyInjection>();
            });

            services.AddJSFacades();

            await builder.Build().RunAsync();
        }
    }
}
