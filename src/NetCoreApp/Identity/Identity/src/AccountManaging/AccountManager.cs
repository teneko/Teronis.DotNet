using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging
{
    public class AccountManager : AccountManager<UserEntity, RoleEntity>, IAccountManager
    {
        public AccountManager(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager,
            ILogger<AccountManager<UserEntity, RoleEntity>>? logger = null)
            : base(userManager, roleManager, logger) { }

        
    }
}
