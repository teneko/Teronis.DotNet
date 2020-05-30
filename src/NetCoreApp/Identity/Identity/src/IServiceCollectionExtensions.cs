using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Identity.Entities;

namespace Teronis.Identity
{
    public static class IServiceCollectionExtensions
    {
        //public static IServiceCollection AddBearerIdentity<DbContextType, UserType, RoleType>(this IServiceCollection services)
        //    where DbContextType : DbContext
        //    where UserType : class, IAccountUserEntity
        //    where RoleType : class, IAccountRoleEntity
        //{
        //    services.AddIdentity<UserType, RoleType>()
        //        .AddEntityFrameworkStores<DbContextType>()
        //        .AddAccountManager<DbContextType, UserType, RoleType>()
        //        .AddBearerSignInManager<DbContextType>();

        //    return services;
        //}
    }
}
