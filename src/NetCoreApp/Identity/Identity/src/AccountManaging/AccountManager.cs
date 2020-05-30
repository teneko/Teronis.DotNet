using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging
{
    public class AccountManager<DbContextType> : AccountManager<DbContextType, UserEntity, RoleEntity>
        where DbContextType : DbContext
    {
        public AccountManager(DbContextType dbContext, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager,
            ILogger<AccountManager<DbContextType, UserEntity, RoleEntity>>? logger = null)
            : base(dbContext, userManager, roleManager, logger) { }


    }
}
