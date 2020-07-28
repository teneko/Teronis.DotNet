using System.Threading.Tasks;
using Teronis.Mvc.ServiceResulting.Generic;

namespace Teronis.Identity.AccountManaging
{
    public interface IAccountManager<UserType, RoleType>
    {
        Task<IServiceResult<RoleType>> CreateRoleAsync(RoleType roleEntity);
        Task<IServiceResult<RoleType>> GetRoleByNameAsync(string roleName);

        Task<IServiceResult<UserType>> CreateUserAsync(UserType userEntity, string password, params string[]? roles);
        Task<IServiceResult<UserType>> GetUserByNameAsync(string userName);
    }
}
