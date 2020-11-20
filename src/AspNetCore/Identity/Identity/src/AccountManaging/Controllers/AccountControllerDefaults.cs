using Teronis.AspNetCore.Identity.AccountManaging.Controllers.Datransjects;
using Teronis.AspNetCore.Identity.Entities;

namespace Teronis.AspNetCore.Identity.AccountManaging.Controllers
{
    public static class AccountControllerDefaults
    {
        private const string policyPrefix = nameof(Teronis) + "." + nameof(Identity) + "." + nameof(Controllers)
            + nameof(AccountController<UserDescriptorDatatransject, UserEntity, UserCreationDatatransject, RoleDescriptorDatatransject, RoleEntity, RoleCreationDatatransject>)
            + ".";

        public const string CanCreateUserPolicy = policyPrefix + "CanCreateUser";
        public const string CanCreateRolePolicy = policyPrefix + "CanCreateRole";
    }
}
