// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging
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
            identityBuilder.addAccountManager<DbContextType, UserType, RoleType>(null);
            return identityBuilder;
        }

        public static IdentityBuilder AddAccountManager<DbContextType>(this IdentityBuilder identityBuilder)
            where DbContextType : DbContext
        {
            var services = identityBuilder.Services;
            services.AddScoped<AccountManager<DbContextType>>();

            static AccountManager<DbContextType> getRequiredService(IServiceProvider serviceProvider) =>
                serviceProvider.GetRequiredService<AccountManager<DbContextType>>();

            identityBuilder.addAccountManager(getRequiredService);
            return identityBuilder;
        }
    }
}
