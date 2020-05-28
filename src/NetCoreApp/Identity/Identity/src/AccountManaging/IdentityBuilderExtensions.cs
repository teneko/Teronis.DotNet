using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.AccountManaging;
using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity
{
    public static partial class IdentityBuilderExtensions
    {
        private static IdentityBuilder addAccountManager<UserType, RoleType>(this IdentityBuilder identityBuilder,
            Func<IServiceProvider, AccountManager<UserType, RoleType>>? getRequiredService)
            where UserType : class, IAccountUserEntity
            where RoleType : class, IAccountRoleEntity
        {
            var services = identityBuilder.Services;

            if (getRequiredService is null) {
                services.AddScoped<AccountManager<UserType, RoleType>>();
            } else {
                services.AddScoped(getRequiredService);
            }

            services.AddScoped<IAccountManager<UserType, RoleType>>(serviceProvider =>
                serviceProvider.GetRequiredService<AccountManager<UserType, RoleType>>());

            return identityBuilder;
        }

        public static IdentityBuilder AddAccountManager<UserType, RoleType>(this IdentityBuilder identityBuilder,
            Func<IServiceProvider, AccountManager<UserType, RoleType>>? getRequiredService)
            where UserType : class, IAccountUserEntity
            where RoleType : class, IAccountRoleEntity
        {
            addAccountManager<UserType, RoleType>(identityBuilder, null);
            return identityBuilder;
        }

        public static IdentityBuilder AddAccountManager(this IdentityBuilder identityBuilder)
        {
            var services = identityBuilder.Services;
            services.AddScoped<AccountManager>();

            AccountManager getRequiredService(IServiceProvider serviceProvider) =>
                serviceProvider.GetRequiredService<AccountManager>();

            services.AddScoped<IAccountManager>(getRequiredService);
            addAccountManager(identityBuilder, getRequiredService);
            return identityBuilder;
        }
    }
}
