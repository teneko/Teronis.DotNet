using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.Entities;
using Teronis.Identity.BearerSignInManaging;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Teronis.Identity
{
    public static partial class IdentityBuilderExtensions
    {
        private static IdentityBuilder addBearerTokenStore<UserType, BearerTokenType, DbContextType>(this IdentityBuilder identityBuilder,
            Func<IServiceProvider, BearerTokenStore<DbContextType, BearerTokenType>>? getRequiredService)
            where UserType : class, IUserEntity
            where BearerTokenType : class, IBearerTokenEntity
            where DbContextType : DbContext
        {
            var services = identityBuilder.Services;

            if (getRequiredService is null) {
                services.AddScoped<BearerTokenStore<DbContextType, BearerTokenType>>();
            } else {
                services.AddScoped(getRequiredService);
            }

            services.AddScoped<IBearerTokenStore<BearerTokenType>>(getRequiredService);
            return identityBuilder;
        }

        public static IdentityBuilder AddBearerTokenStore<UserType, BearerTokenType, DbContextType>(this IdentityBuilder identityBuilder)
            where UserType : class, IUserEntity
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

            BearerTokenStore<DbContextType> getRequiredService(IServiceProvider serviceProvider) =>
               serviceProvider.GetRequiredService<BearerTokenStore<DbContextType>>();

            services.AddScoped<IBearerTokenStore>(getRequiredService);
            addBearerTokenStore<UserEntity, BearerTokenEntity, DbContextType>(identityBuilder, getRequiredService);
            return identityBuilder;
        }
    }
}
