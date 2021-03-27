// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Dynamic.Locality;
using Teronis.Microsoft.JSInterop.Dynamic.Module;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.NUnit.TaskTests;
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

            services.AddJSCustomFacadeActivator(options => {
                options.JSFacadeDictionaryConfiguration
                    .Add(jsObjectReference => new ModuleActivationViaManualConstruction(jsObjectReference))
                    .Add<ModuleActivationViaDependencyInjection>();
            });

            services.AddScoped<ITaskTestCaseBlock>(serviceProvider => JSFacadesTests.Instance);
            services.AddJSFacades();

            await builder.Build().RunAsync();
        }
    }
}
