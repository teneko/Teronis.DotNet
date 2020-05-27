using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Teronis.Identity.AccountManaging.Datatransjects;
using Teronis.Identity.Datransjects;
using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging
{
    public class AccountManager : AccountManager<UserDescriptorDatatransject, UserEntity, UserCreationDatatransject, RoleDescriptorDatatransject, RoleEntity, RoleCreationDatatransject>, IAccountManager
    {
        protected AccountManager(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager,
            ILogger<AccountManager<UserDescriptorDatatransject, UserEntity, UserCreationDatatransject, RoleDescriptorDatatransject, RoleEntity, RoleCreationDatatransject>>? logger = null)
            : base(userManager, roleManager, logger) { }

        protected override RoleEntity CreateRoleEntity(RoleDescriptorDatatransject roleDescriptor) =>
            new RoleEntity(roleDescriptor.Role);

        protected override UserEntity CreateUserEntity(UserDescriptorDatatransject userDescriptor)
        {
            return new UserEntity(userDescriptor.UserName) {
                Email = userDescriptor.Email,
                PhoneNumber = userDescriptor.PhoneNumber,
            };
        }

        protected override RoleCreationDatatransject CreateRoleCreationObject(RoleEntity roleEntity)
        {
            return new RoleCreationDatatransject() {
                Role = roleEntity.Id
            };
        }

        protected override UserCreationDatatransject CreateUserCreationObject(UserEntity userEntity)
        {
            return new UserCreationDatatransject() {
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                PhoneNumber = userEntity.PhoneNumber,
            };
        }
    }
}
