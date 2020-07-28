using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.Entities;

namespace Teronis.Identity.Bearer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.SeedIdentityAsync(async (serviceProvider, accountManager) => {
                var dbContext = serviceProvider.GetRequiredService<BearerIdentityDbContext>();
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                var roleEntity = new RoleEntity(TeronisIdentityBearerExampleDefaults.AdministratorRoleName);
                await accountManager.CreateRoleIfNotExistsAsync(roleEntity);
                var userEntity = new UserEntity(TeronisIdentityBearerExampleDefaults.AdministratorUserName);
                await accountManager.CreateUserIfNotExistsAsync(userEntity, "aA#123", TeronisIdentityBearerExampleDefaults.AdministratorRoleName);
            });

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
