using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.Entities;
using Teronis.Identity.BearerSignInManaging;

namespace Teronis.Identity
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddSignInServiceStores<DbContextType>([DisallowNull]this IdentityBuilder identityBuilder)
            where DbContextType : DbContext
        {
            identityBuilder.Services.AddScoped<IRefreshTokenStore<RefreshTokenEntity>, RefreshTokenStore<DbContextType, RefreshTokenEntity>>();
            return identityBuilder;
        }
    }
}
