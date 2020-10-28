using System.Threading.Tasks;
using Teronis.Identity.Entities;

namespace Teronis.Identity.AccountManaging
{
    public static class IAccountManagerExtensions
    {
        public static async Task<RoleType> CreateRoleIfNotExistsAsync<RoleType, UserType>(
            this IAccountManager<UserType, RoleType> accountManager, RoleType roleEntity)
            where RoleType : IAccountRoleEntity
        {
            try {
                var role = await accountManager.CreateRoleAsync(roleEntity);
                return role;
            } catch (RoleAlreadyCreatedException) {
                return await accountManager.GetRoleByNameAsync(roleEntity.RoleName);
            }
        }

        public static async Task<UserType> CreateUserIfNotExistsAsync<RoleType, UserType>(
            this IAccountManager<UserType, RoleType> accountManager,
            UserType userEntity, string password, params string[]? roles)
            where UserType : IAccountUserEntity
        {
            try {
                var user = await accountManager.CreateUserAsync(userEntity, password, roles);
                return user;
            } catch (UserAlreadyCreatedException) {
                return await accountManager.GetUserByNameAsync(userEntity.UserName);
            }
        }
    }
}
