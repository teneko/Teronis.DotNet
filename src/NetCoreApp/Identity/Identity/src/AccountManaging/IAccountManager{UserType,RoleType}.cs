using System.Threading.Tasks;
using Teronis.Mvc.ServiceResulting.Generic;

namespace Teronis.Identity.AccountManaging
{
    public interface IAccountManager<UserType, RoleType>
    {
        Task<RoleType> CreateRoleAsync(RoleType roleEntity);
        Task<RoleType> GetRoleByNameAsync(string roleName);

        Task<UserType> CreateUserAsync(UserType userEntity, string password, params string[]? roles);
        Task<UserType> GetUserByNameAsync(string userName);
    }
}
