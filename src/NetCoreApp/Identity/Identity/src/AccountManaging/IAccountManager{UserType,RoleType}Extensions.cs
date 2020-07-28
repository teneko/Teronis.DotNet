using System.Threading.Tasks;
using Teronis.Identity.Entities;
using Teronis.Mvc.ServiceResulting;
using Teronis.Mvc.ServiceResulting.Generic;
using Teronis.ObjectModel.Annotations;

namespace Teronis.Identity.AccountManaging
{
    public static class IAccountManagerExtensions
    {
        public static async Task<IServiceResult<RoleType>> CreateRoleIfNotExistsAsync<RoleType, UserType>(
            this IAccountManager<UserType, RoleType> accountManager, RoleType roleEntity)
            where RoleType : IAccountRoleEntity
        {
            var result = await accountManager.CreateRoleAsync(roleEntity);

            if (result.Success(AccountManagerErrorCodes.RoleAlreadyCreated.GetStringValue())) {
                return await accountManager.GetRoleByNameAsync(roleEntity.RoleName);
            }

            return result;
        }

        public static async Task<IServiceResult<UserType>> CreateUserIfNotExistsAsync<RoleType, UserType>(
            this IAccountManager<UserType, RoleType> accountManager,
            UserType userEntity, string password, params string[]? roles)
            where UserType : IAccountUserEntity
        {
            var result = await accountManager.CreateUserAsync(userEntity, password, roles);

            if (result.Success(AccountManagerErrorCodes.UserAlreadyCreated.GetStringValue())) {
                return await accountManager.GetUserByNameAsync(userEntity.UserName);
            }

            return result;
        }
    }
}
