using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.Entities;

namespace Teronis.Identity
{
    public static partial class IdentityBuilderExtensions
    {
        private static IdentityBuilder addAccountManager<DbContextType, UserType, RoleType>(this IdentityBuilder identityBuilder,
            Func<IServiceProvider, AccountManager<DbContextType, UserType, RoleType>>? getRequiredService)
            where DbContextType : DbContext
            where UserType : class, IAccountUserEntity
            where RoleType : class, IAccountRoleEntity
        {
            var services = identityBuilder.Services;

            if (getRequiredService is null) {
                services.AddScoped<AccountManager<DbContextType, UserType, RoleType>>();
            } else {
                services.AddScoped(getRequiredService);
            }

            services.AddScoped<IAccountManager<UserType, RoleType>>(serviceProvider =>
                serviceProvider.GetRequiredService<AccountManager<DbContextType, UserType, RoleType>>());

            return identityBuilder;
        }

        public static IdentityBuilder AddAccountManager<DbContextType, UserType, RoleType>(this IdentityBuilder identityBuilder)
            where DbContextType : DbContext
            where UserType : class, IAccountUserEntity
            where RoleType : class, IAccountRoleEntity
        {
            addAccountManager<DbContextType, UserType, RoleType>(identityBuilder, null);
            return identityBuilder;
        }

        public static IdentityBuilder AddAccountManager<DbContextType>(this IdentityBuilder identityBuilder)
            where DbContextType : DbContext
        {
            var services = identityBuilder.Services;
            services.AddScoped<AccountManager<DbContextType>>();

            static AccountManager<DbContextType> getRequiredService(IServiceProvider serviceProvider) =>
                serviceProvider.GetRequiredService<AccountManager<DbContextType>>();

            addAccountManager(identityBuilder, getRequiredService);
            return identityBuilder;
        }
    }
}
