using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IHostExtensions
    {
        public static async Task<IHost> SeedIdentityAsync<UserType, RoleType>(this IHost host, Func<IServiceProvider, IAccountManager<UserType, RoleType>, Task> handler)
        {
            await host.CreateScopeAsync(serviceProvider => {
                var accountManager = serviceProvider.GetRequiredService<IAccountManager<UserType, RoleType>>();
                return handler(serviceProvider, accountManager);
            });

            return host;
        }

        public static Task<IHost> SeedIdentityAsync(this IHost host, Func<IServiceProvider, IAccountManager<UserEntity, RoleEntity>, Task> handler) =>
            SeedIdentityAsync<UserEntity, RoleEntity>(host, handler);
    }
}
