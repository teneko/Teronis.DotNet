// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teronis.AspNetCore.Identity.Bearer.Stores;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.Bearer
{
    public static partial class IdentityBuilderExtensions
    {
        private static IdentityBuilder addBearerTokenStore<UserType, BearerTokenType, DbContextType>(this IdentityBuilder identityBuilder,
            Func<IServiceProvider, BearerTokenStore<DbContextType, BearerTokenType>>? getRequiredService)
            where UserType : class, IBearerUserEntity
            where BearerTokenType : class, IBearerTokenEntity
            where DbContextType : DbContext
        {
            var services = identityBuilder.Services;

            if (getRequiredService is null) {
                services.AddScoped<BearerTokenStore<DbContextType, BearerTokenType>>();
                getRequiredService = serviceProvider => serviceProvider.GetRequiredService<BearerTokenStore<DbContextType, BearerTokenType>>();
            } else {
                services.AddScoped(getRequiredService);
            }

            services.AddScoped<IBearerTokenStore<BearerTokenType>>(getRequiredService);
            return identityBuilder;
        }

        public static IdentityBuilder AddBearerTokenStore<UserType, BearerTokenType, DbContextType>(this IdentityBuilder identityBuilder)
            where UserType : class, IBearerUserEntity
            where BearerTokenType : class, IBearerTokenEntity
            where DbContextType : DbContext
        {
            var services = identityBuilder.Services;
            addBearerTokenStore<UserType, BearerTokenType, DbContextType>(identityBuilder, null);
            return identityBuilder;
        }

        public static IdentityBuilder AddBearerTokenStore<DbContextType>(this IdentityBuilder identityBuilder)
            where DbContextType : DbContext
        {
            var services = identityBuilder.Services;
            services.AddScoped<BearerTokenStore<DbContextType>>();

            static BearerTokenStore<DbContextType> getRequiredService(IServiceProvider serviceProvider) =>
               serviceProvider.GetRequiredService<BearerTokenStore<DbContextType>>();

            services.AddScoped<IBearerTokenStore>(getRequiredService);
            addBearerTokenStore<UserEntity, BearerTokenEntity, DbContextType>(identityBuilder, getRequiredService);
            return identityBuilder;
        }
    }
}
