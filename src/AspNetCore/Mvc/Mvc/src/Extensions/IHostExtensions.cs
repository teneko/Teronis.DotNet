using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IHostExtensions
    {
        public static IHost CreateScope(this IHost host, Action<IServiceProvider> handler)
        {
            var serviceScopeFactory = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope()) {
                handler(scope.ServiceProvider);
            }

            return host;
        }

        public static async Task<IHost> CreateScopeAsync(this IHost host, Func<IServiceProvider, Task> handler)
        {
            var serviceScopeFactory = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope()) {
                await handler(scope.ServiceProvider);
            }

            return host;
        }
    }
}
